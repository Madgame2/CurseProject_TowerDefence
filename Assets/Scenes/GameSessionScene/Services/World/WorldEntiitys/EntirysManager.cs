using Common.Services.Net.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private EntityDataBase_enum entityDataBase_enum;
    [SerializeField] private EntityDataBase entityDataBase;
    [SerializeField] private EnityFactory _factory;

    [Inject] private WebSocketModule _socket;
    [Inject] private DiContainer _container;

    private Dictionary<string, GameObject> entities = new Dictionary<string, GameObject>();
    private EntityFactory _factory_enum;

    private void Awake()
    {
        _factory_enum = new EntityManager.EntityFactory(entityDataBase_enum, _container);
    }

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

    internal void handleEnityEvnet(EntityEvent entity)
    {
        switch (entity.EnventType)
        {
            case EntityEventType.SPAWN:
                GameObject newObject =  _factory_enum.Create(entity);

                entities.Add(entity.EnityId, newObject);
                break;
            case EntityEventType.TERMINATE:

                break;
        }
    }


    private class EntityFactory
    {
        private EntityDataBase_enum _db;
        private DiContainer _container;

        public EntityFactory(EntityDataBase_enum db, DiContainer container)
        {
            _db = db;
            _container = container;
        }

        public GameObject Create(EntityEvent entity)
        {
            var config = _db.Get(entity.EnityType);
            if (config == null) return null;

            GameObject go = _container.InstantiatePrefab(config.prefab);

            switch (entity.EnityType)
            {
                case EntityesEnum.GrossCannonInBuild:
                    HandleGrossCannon(go, entity);
                    break;
            }

            return go;
        }

        private void HandleGrossCannon(GameObject go, EntityEvent entity)
        {
            var data = ConvertData<GrossCannonInBuildData>(entity.Data);

            if (data == null)
            {
                Debug.LogError("Failed to parse GrossCannon data");
                return;
            }

            // пример инициализации
            go.transform.position = new Vector3(data.possiton.X*10, 0, data.possiton.Y * 10);

            //var component = go.GetComponent<GrossCannonInBuildView>();
            //if (component != null)
            //{
            //    component.SetProgress(data.progress);
            //}
        }

        private T ConvertData<T>(object data)
        {
            // если пришёл уже нужный тип
            if (data is T typed)
                return typed;

            // если это JObject (чаще всего при Newtonsoft)
            if (data is Newtonsoft.Json.Linq.JObject jObj)
                return jObj.ToObject<T>();

            // fallback
            try
            {
                return JsonConvert.DeserializeObject<T>(data.ToString());
            }
            catch
            {
                return default;
            }
        }
    }
}
