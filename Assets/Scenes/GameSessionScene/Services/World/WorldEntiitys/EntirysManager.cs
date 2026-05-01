using Common.Services.Net.Modules;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Zenject;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private EntityDataBase entityDataBase;
    [SerializeField] private EnityFactory _factory;

    [Inject] private WebSocketModule _socket;

    private Dictionary<string, GameObject> entities = new Dictionary<string, GameObject>();


    public void Start()
    {
        _socket.On("EntityReg", EntityRegHandler);
    }

    private void OnDisable()
    {
        _socket.Off("EntityReg", EntityRegHandler);
    }

    private async Task EntityRegHandler(string arg)
    {
        Debug.Log(arg);
        var jObject = JObject.Parse(arg);

        EntityRegBasePacket packet;

        if (jObject["hp"] != null)
            packet = jObject.ToObject<EntityRegWithHpPacket>();
        else
            packet = jObject.ToObject<EntityRegBasePacket>();

        RegNewEntity(packet);


        _ = _socket.Send("EntityApply", new { });
    }

    public void RegNewEntity(EntityRegBasePacket packet)
    {
        var entityConfig = entityDataBase.getById(packet.structId);
        if (entityConfig == null) return;

        Vector3 position = new Vector3(
            packet.position.X,
            0,
            packet.position.Y
            );

        var result = _factory.PlaysEntity(entityConfig, position);
        if(result == null) return;

        entities[packet.id] = result;
    }
}
