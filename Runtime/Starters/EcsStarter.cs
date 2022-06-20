using DesertImage.Managers;
using Group;
using UnityEngine;

namespace DesertImage.ECS
{
    public class EcsStarter : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] modules;

        protected Core Core;

        protected SystemsManager SystemsManager;

        protected virtual void Awake()
        {
            Core = new Core();

            Core.Add<ManagerUpdate>();

            var world = Core.Add<World>();

            var groupsManager = Core.Add<GroupsManager>();
            groupsManager.Init(world);

            SystemsManager = Core.Add(new SystemsManager(world));

            Core.Add<FactorySpawn>();
            Core.Add<FactorySound>();
            Core.Add<FactoryFx>();

            InitComponents();

            InitModules();

            InitSystems();
        }

        private void Start()
        {
            Core.OnStart();
        }

        private void InitComponents()
        {
            var components = GetComponents<IComponentWrapper>();

            var entity = Core.Get<World>().GetNewEntity();

            foreach (var componentWrapper in components)
            {
                componentWrapper.Link(entity);
            }
        }

        protected virtual void InitModules()
        {
            if (modules == null) return;

            foreach (var module in modules)
            {
                if (!module) continue;

                Core.Add(module);
            }
        }

        protected virtual void InitSystems()
        {
        }

        private void OnDestroy()
        {
            Core.Dispose();
        }
    }
}