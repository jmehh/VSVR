using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BlockGroup : MonoBehaviour
{
    public bool randomiseSubBlocks = false;

    public List<BlockManager> GetBlocks()
    {
        List<BlockManager> managers = new List<BlockManager>();

        List<BlockGroup> groups = new List<BlockGroup>();
        List<BlockManager> childManagers = new List<BlockManager>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (!child.gameObject.activeSelf)
            {
                continue;
            }
            if (child.gameObject.TryGetComponent(out BlockGroup group))
            {
                groups.Add(group);
            } 
            else if (child.gameObject.TryGetComponent(out BlockManager manager))
            {
                childManagers.Add(manager);
            }
        }

        // Block manager in current object
        if (TryGetComponent(out BlockManager blockManager))
        {
            managers.Add(blockManager);
        } 
        else if (groups.Count > 0)
        {
            if (randomiseSubBlocks)
            {
                var rnd = new System.Random();
                groups = groups.OrderBy(x => rnd.Next()).Take(groups.Count).ToList();
            }

            foreach (var group in groups)
            {
                managers.AddRange(group.GetBlocks());
            }
        } 
        else if (childManagers.Count > 0)
        {
            if (randomiseSubBlocks)
            {
                var rnd = new System.Random();
                childManagers = childManagers.OrderBy(x => rnd.Next()).Take(childManagers.Count).ToList();
            }
            managers.AddRange(childManagers);
        } 

        return managers;

    }
}
