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
                GameObject enityObject = entities.GetValueOrDefault(entity.EnityId);
                if (!enityObject) return;

                Destroy(enityObject);
                entities.Remove(entity.EnityId);
                break;
            case EntityEventType.UPDATE:
                handleUpdateEntity(entity.EnityId, entity.EnityType, entity.Data);
                break;
        }
    }


    private void handleUpdateEntity(string enityId, EntityesEnum enityType, object data)
    {
        GameObject gameObject = entities.GetValueOrDefault(enityId);
        if(gameObject == null) return;

        switch (enityType)
        {
            case EntityesEnum.GrossCannonInBuild:
                {
                    if(gameObject.TryGetComponent<GrossCannonInBuild>(out GrossCannonInBuild grossCannonInBuild))
                    {
                        var updates = ConvertData<GrossCannonInBuildUpdate>(data);
                        grossCannonInBuild.SetProgress(updates.progress);
                    }
                    break;
                }
            case EntityesEnum.GrossCannon:
                {

                    break;
                }
            case EntityesEnum.TeslaTowerBuild:
                {
                    if (gameObject.TryGetComponent<TeslaTowerInBuild>(out TeslaTowerInBuild grossCannonInBuild))
                    {
                        var updates = ConvertData<TeslaTowerInBuildUpdate>(data);
                        grossCannonInBuild.SetProgress(updates.progress);
                    }
                    break;
                }

            case EntityesEnum.CampInBuild:
                {
                    if (gameObject.TryGetComponent<CampInBuild>(out CampInBuild grossCannonInBuild))
                    {
                        var updates = ConvertData<CampInBuildUpdate>(data);
                        grossCannonInBuild.SetProgress(updates.progress);
                    }
                    break;
                }

            case EntityesEnum.RootHouse:
                {
                    if(gameObject.TryGetComponent<RooObjectStatesView>(out RooObjectStatesView view))
                    {
                        var updates = ConvertData<RootHouseUpdate>(data);
                        view.SetHealth(updates.health_present);
                    }
                }
                break;
        }
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
                case EntityesEnum.GrossCannon:
                    HandleGrossCannonBuilded(go, entity);
                    break;

                case EntityesEnum.TeslaTowerBuild:
                    HandleTeslaTowerBuild(go, entity);
                    break;
                case EntityesEnum.TeslaTower:
                    HandleTeslaTower(go, entity);
                    break;

                case EntityesEnum.CampInBuild:
                    HandleCampBuild(go, entity);
                    break;
                case EntityesEnum.Camp:
                    HandleCamp(go, entity);
                    break;
            }

            return go;
        }

        private void HandleCamp(GameObject go, EntityEvent entity)
        {
            var data = ConvertData<CampData>(entity.Data);

            if (data == null)
            {
                Debug.LogError("Failed to parse GrossCannon data");
                return;
            }

            // пример инициализации
            go.transform.position = new Vector3(data.possiton.X * 10, 0, data.possiton.Y * 10);
        }

        private void HandleCampBuild(GameObject go, EntityEvent entity)
        {
            var data = ConvertData<CampInBuildData>(entity.Data);

            if (data == null)
            {
                Debug.LogError("Failed to parse GrossCannon data");
                return;
            }

            // пример инициализации
            go.transform.position = new Vector3(data.possiton.X * 10, 0, data.possiton.Y * 10);
        }

        private void HandleTeslaTower(GameObject go, EntityEvent entity)
        {
            var data = ConvertData<TeslaTowerData>(entity.Data);

            if (data == null)
            {
                Debug.LogError("Failed to parse GrossCannon data");
                return;
            }

            // пример инициализации
            go.transform.position = new Vector3(data.possiton.X * 10, 0, data.possiton.Y * 10);

            //var component = go.GetComponent<GrossCannonInBuildView>();
            //if (component != null)
            //{
            //    component.SetProgress(data.progress);
            //}
        }

        private void HandleTeslaTowerBuild(GameObject go, EntityEvent entity)
        {
            var data = ConvertData<TeslaTowerBuildData>(entity.Data);

            if (data == null)
            {
                Debug.LogError("Failed to parse GrossCannon data");
                return;
            }

            // пример инициализации
            go.transform.position = new Vector3(data.possiton.X * 10, 0, data.possiton.Y * 10);

            //var component = go.GetComponent<GrossCannonInBuildView>();
            //if (component != null)
            //{
            //    component.SetProgress(data.progress);
            //}
        }

        private void HandleGrossCannonBuilded(GameObject go, EntityEvent entity)
        {
            var data = ConvertData<GrossCannonData>(entity.Data);

            if (data == null)
            {
                Debug.LogError("Failed to parse GrossCannon data");
                return;
            }

            // пример инициализации
            go.transform.position = new Vector3(data.possiton.X * 10, 0, data.possiton.Y * 10);

            //var component = go.GetComponent<GrossCannonInBuildView>();
            //if (component != null)
            //{
            //    component.SetProgress(data.progress);
            //}
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
