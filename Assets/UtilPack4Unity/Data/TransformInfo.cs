using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class TransformInfo
    {
        public TypeUtils.Json.Vec3 Position;
        public TypeUtils.Json.Vec3 EulerAngles;
        public TypeUtils.Json.Vec3 LossyScale;
        public TypeUtils.Json.Vec3 LocalPosition;
        public TypeUtils.Json.Vec3 LocalEulerAngles;
        public TypeUtils.Json.Vec3 LocalScale;
        public static TransformInfo identity
        {
            get {
                return new TransformInfo(Vector3.zero, Vector3.zero, Vector3.one, Vector3.zero, Vector3.zero, Vector3.one);
            }
        }

        public TransformInfo()
        {

        }

        public TransformInfo(Transform transform)
        {
            this.LocalPosition = TypeUtils.Json.Convert.Vector3ToVec3(transform.localPosition);
            this.LocalEulerAngles = TypeUtils.Json.Convert.Vector3ToVec3(transform.localEulerAngles);
            this.LocalScale = TypeUtils.Json.Convert.Vector3ToVec3(transform.localScale);
            this.Position = TypeUtils.Json.Convert.Vector3ToVec3(transform.position);
            this.EulerAngles = TypeUtils.Json.Convert.Vector3ToVec3(transform.eulerAngles);
            this.LossyScale = TypeUtils.Json.Convert.Vector3ToVec3(transform.lossyScale);
        }

        public TransformInfo(Vector3 position, Vector3 eulerAngles, Vector3 lossyScale, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
        {
            this.LocalPosition = TypeUtils.Json.Convert.Vector3ToVec3(localPosition);
            this.LocalEulerAngles = TypeUtils.Json.Convert.Vector3ToVec3(localEulerAngles);
            this.LocalScale = TypeUtils.Json.Convert.Vector3ToVec3(localScale);
            this.Position = TypeUtils.Json.Convert.Vector3ToVec3(position);
            this.EulerAngles = TypeUtils.Json.Convert.Vector3ToVec3(eulerAngles);
            this.LossyScale = TypeUtils.Json.Convert.Vector3ToVec3(lossyScale);
        }
    }

}
