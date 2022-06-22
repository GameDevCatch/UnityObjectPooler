# Unity-Object-Pooler

Simple object pooler using Unity's new 2021 object pooling API

## SETUP

1. Create An Empty Gameobject
2. Apply ObjectPooler Script
3. Make Sure To Put Using Catch.Utils In Your Script
4. Call API Using Its Singleton
5. Profit!

## HOW TO USE

There's 4 streamlined functions you can call.

1. WarmPool(GameObject prefab, Int size, Int maxSize)

Takes the supplied args and creates a pool with it.

1. Spawn(GameObject prefab, Vector3 pos, Quaternion rot)

Instead of Instantiate.

1. Release(GameObject obj)

Instead of Destroy.

1. ClearAllPools()

Releases all the pools objects.
