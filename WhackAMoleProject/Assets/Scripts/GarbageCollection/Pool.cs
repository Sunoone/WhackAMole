using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarbageCollection
{
    public class Pool : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Poolable prefab;
#pragma warning restore 649
        [SerializeField]
        private bool _willGrow = true;
        [SerializeField]
        private int _size = 10;

        private List<Poolable> _items = new List<Poolable>();
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < _size; i++)
            {
                var item = SpawnItem();
                item.gameObject.SetActive(false);
            }          
        }

        protected virtual Poolable SpawnItem()
        {
            var item = Poolable.Create(prefab, this);
            _items.Add(item);
            return item;
        }

        public Poolable GetItem()
        {
            int length = _items.Count;
            for (int i = 0; i < length; i++)
            {
                var item = _items[i];
                if (!item.gameObject.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                    return item;
                }
            }
            if (_willGrow)
                return SpawnItem();
            return null;
        }

        public void PoolItem(Poolable item)
        {
            item.transform.SetParent(transform);
            item.gameObject.SetActive(false);
        }
    }
}
