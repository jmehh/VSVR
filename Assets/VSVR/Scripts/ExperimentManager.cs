using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XR;

public class ExperimentManager : MonoBehaviour
{

    private int nBlocks;
    private int currentBlock = -1;
    private List<BlockManager> blocks = new List<BlockManager>();
    
    void Start()
    {
        // Get blocks

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                if (child.gameObject.TryGetComponent(out BlockGroup group))
                {
                    blocks.AddRange(group.GetBlocks());
                }
            }
        }

        foreach(var block in blocks)
        {
            block.blockFinished += BlockFinished;
            print(block.blockName);
        }

        NextBlock();
    }

    public void NextBlock()
    {
        currentBlock++;
        if(currentBlock < blocks.Count)
        {
            // update DataManager with new block
            DataManager.NewBlock(blocks[currentBlock].blockName);

            blocks[currentBlock].StartBlock();
        } 
        else
        {
            //experiment finished
        }
    }

    public void BlockFinished()
    {

        DataManager.WriteBlockData();
        NextBlock();
    }

}
