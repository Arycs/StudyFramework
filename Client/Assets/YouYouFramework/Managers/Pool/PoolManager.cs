using System;
using System.Collections.Generic;
namespace YouYou
{
    public class PoolManager : ManagerBase
    {

        /// <summary>
        /// 类对象池
        /// </summary>
        public ClassObjectPool ClassObjectPool { get; private set; }
        /// <summary>
        /// 游戏物体对象池
        /// </summary>
        public GameObjectPool GameObjectPool { get; private set; }

        /// <summary>
        /// 资源包池
        /// </summary>
        public ResourcePool AssetBundlePool { get; private set; }

        /// <summary>
        /// 分类资源池
        /// </summary>
        public Dictionary<AssetCategory, ResourcePool> AssetPool { get; private set; }

        public PoolManager()
        {
            ClassObjectPool = new ClassObjectPool();
            GameObjectPool = new GameObjectPool();
            
            AssetBundlePool = new ResourcePool("AssetBundlePool");
            
            AssetPool = new Dictionary<AssetCategory, ResourcePool>();
            //确保游戏刚开始运行的时候,分类资源池已经初始化号了
            var enumerator = Enum.GetValues(typeof(AssetCategory)).GetEnumerator();
            while (enumerator.MoveNext())
            {
                AssetCategory assetCategory = (AssetCategory) enumerator.Current;
                if (assetCategory == AssetCategory.None)
                {
                    continue;
                }
                AssetPool[assetCategory] = new ResourcePool(assetCategory.ToString());
            }
        }

        /// <summary>
        /// 释放类对象池
        /// </summary>
        public void ReleaseClassObjectPool()
        {
            ClassObjectPool.Release();
        }

        /// <summary>
        /// 释放资源包池
        /// </summary>
        public void ReleaseAssetBundlePool()
        {
            AssetBundlePool.Release();
        }

        /// <summary>
        /// 释放分类资源池中的所有资源
        /// </summary>
        public void ReleaseAssetPool()
        {
            var enumerator = Enum.GetValues(typeof(AssetCategory)).GetEnumerator();
            while (enumerator.MoveNext())
            {
                AssetCategory assetCategory = (AssetCategory) enumerator.Current;
                if (assetCategory == AssetCategory.None)
                {
                    continue;
                }

                AssetPool[assetCategory].Release();
            }
        }


        public void Dispose()
        {
            ClassObjectPool.Dispose();
            GameObjectPool.Dispose();
        }
        
    }
}