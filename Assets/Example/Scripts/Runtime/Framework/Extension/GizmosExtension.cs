using System;
using UnityEngine;

namespace GameMain.Runtime
{
    public static class GizmosExtension
    {
        public static bool IsOpenGizmosDraw = true;
        
        public static void DrawBox(this Transform transform, float x, float y, Color color)
        {
            if (!IsOpenGizmosDraw) { return; }
            DrawBox(transform.position, transform.rotation, x, y, color);
        }

        public static void DrawCircle(this Transform transform, float radius, Color color)
        {
            if (!IsOpenGizmosDraw) { return; }
            DrawCircle(transform.position,radius, color);
        }

        public static void DrawGizmosData(this GizmosData data)
        {
            if (!IsOpenGizmosDraw) { return; }
            if (data.Type == GizmosData.ShapeType.Circle)
            {
                if (Mathf.Approximately(data.Angle,0f) || Mathf.Approximately(data.Angle,360f))
                {
                    DrawCircle(data.Transform.position, data.Radius,data.Color);
                }
                else
                {
                    DrawSector(data.Transform.position, data.Transform.rotation, data.Radius, data.Angle, data.Color);
                }
            }
            else if(data.Type == GizmosData.ShapeType.Box)
            {
                DrawBox(data.Transform.position, data.Transform.rotation, data.X, data.Y, data.Color);
            }
            else if(data.Type == GizmosData.ShapeType.Line)
            {
                DrawLine(data.Transform.position, data.TargetPos, data.Color);
            }
            else if(data.Type == GizmosData.ShapeType.Lines)
            {
                DrawLines(data.Points, data.Color);
            }
        }

        private static void DrawLine(Vector3 position, Vector3 targetPosition,Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(position, targetPosition);
        }
        
        private static void DrawLines(Vector3[] points,Color color)
        {
            if (points == null || points.Length < 2)
                return;
            Gizmos.color = color;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }

        private static void DrawBox(Vector3 position, Quaternion rotation, float x, float y, Color color)
        {
            Gizmos.color = color;

            // 计算盒子的四个角的位置
            Vector3 topLeft = position + rotation * new Vector3(-x / 2, 0.1f, y / 2);
            Vector3 topRight = position + rotation * new Vector3(x / 2, 0.1f, y / 2);
            Vector3 bottomLeft = position + rotation * new Vector3(-x / 2, 0.1f, -y / 2);
            Vector3 bottomRight = position + rotation * new Vector3(x / 2, 0.1f, -y / 2);

            // 绘制盒子的四条边
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
        
        private static void DrawCircle(Vector3 position, float radius, Color color)
        {
            Gizmos.color = color;

            var vertexCount = 50;
                        
            float deltaTheta = (2f * Mathf.PI) / vertexCount;
            float theta = 0f;

            Vector3 center = new Vector3(position.x, position.y + 0.1F, position.z);
            Vector3 oldPos = center; 

            for (int i = 0; i < vertexCount+1; i++)
            {
                Vector3 newPos = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                Gizmos.DrawLine(oldPos, newPos + center);
                oldPos = newPos + center;
                theta += deltaTheta;
            }
        }
        
        private static void DrawSector(Vector3 position, Quaternion rotation, float radius, float angle, Color color)
        {
            Gizmos.color = color;

            var vertexCount = 50;
            
            float angleInRadians = angle * Mathf.Deg2Rad;
            float deltaTheta = angleInRadians / vertexCount;
            
            Vector3 forward = rotation * Vector3.forward;
            Vector3 right = rotation * Vector3.right;

            Vector3 center = new Vector3(position.x, position.y, position.z);
            Vector3 oldPos = center; 
            
            float startAngle = -angleInRadians / 2;
            
            for (int i = 0; i <= vertexCount; i++)
            {
                float theta = startAngle + i * deltaTheta;

                float x = Mathf.Cos(theta);
                float z = Mathf.Sin(theta);
                
                Vector3 newPos = radius * (x * forward + z * right);
                Gizmos.DrawLine(oldPos, newPos + center);
                oldPos = newPos + center;
            }
            
            Vector3 startPos = radius * (Mathf.Cos(startAngle) * forward + Mathf.Sin(startAngle) * right) + center;
            Vector3 endPos = radius * (Mathf.Cos(startAngle + angleInRadians) * forward + Mathf.Sin(startAngle + angleInRadians) * right) + center;
            Gizmos.DrawLine(center, startPos);
            Gizmos.DrawLine(center, endPos);
        }
        
    }
}