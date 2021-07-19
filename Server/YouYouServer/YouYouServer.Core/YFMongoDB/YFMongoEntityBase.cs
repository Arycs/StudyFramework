using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Core.YFMongoDB
{
    /// <summary>
    /// MongoEntity基类, 特性是忽略没有匹配的字段
    /// </summary>
    [BsonIgnoreExtraElements]
    public class YFMongoEntityBase
    {
        [JsonConverter(typeof(YFObjectIdConverter))]
        /// <summary>
        /// MongoDB自动生成的ID,后续可能需要做映射
        /// </summary>
        public ObjectId Id;

        /// <summary>
        /// MongoDB里会自动有一个ID, 因此使用YFId当主键
        /// </summary>
        public long YFId;

        /// <summary>
        /// 状态
        /// </summary>
        public DataStatus Status;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime;

    }
}
