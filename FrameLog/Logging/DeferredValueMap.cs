﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameLog.Logging
{
    public class DeferredValueMap<TContainer>
    {
        private Dictionary<TContainer, Dictionary<string, Func<object>>> map;

        public DeferredValueMap()
        {
            map = new Dictionary<TContainer, Dictionary<string, Func<object>>>();
        }

        public void Store(TContainer container, string key, Func<object> future)
        {
            var subMap = getSubmap(container);
            subMap[key] = future;
        }
        public bool HasContainer(TContainer container)
        {
            return map.ContainsKey(container);
        }
        public IDictionary<string, object> CalculateAndRetrieve(TContainer container)
        {
            var subMap = map[container];
            return subMap.ToDictionary(kv => kv.Key, kv => kv.Value());
        }

        private Dictionary<string, Func<object>> getSubmap(TContainer container)
        {
            Dictionary<string, Func<object>> subMap;
            if (!map.TryGetValue(container, out subMap))
            {
                map[container] = subMap = new Dictionary<string, Func<object>>();
            }
            return subMap;
        }
    }
}
