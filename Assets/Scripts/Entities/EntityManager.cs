using CuscuzBane.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CuscuzBane.Entities
{
    public static class EntityManager
    {
        private static List<Entity> entities;

        public static ReadOnlyCollection<Entity> Entities => new ReadOnlyCollection<Entity>(entities);

        static EntityManager()
        {
            entities = new List<Entity>();
        }

        public static void Register(Entity entity)
        {
            entities.Add(entity);
        }

        public static void Remove(Entity entity)
        {
            entities.Remove(entity);
        }
    }
}
