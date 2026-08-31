using UnityEngine;

namespace Editor.Interfaces.Math
{
    public static class MathTools
    {
        public static Vector2Int GlobalPosToChunkPosition(float worldX, float worldY, int chunkSize, Vector2 pivot)
        {
            float offsetX = worldX + (pivot.x * chunkSize);
            float offsetY = worldY + (pivot.y * chunkSize);
            
            
            int chunkX = Mathf.FloorToInt(offsetX / chunkSize);
            int chunkY = Mathf.FloorToInt(offsetY / chunkSize);

            return new Vector2Int(chunkX, chunkY);
        }
        
        public static float GetHeightOnTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 targetPos)
        {
            float det = (p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z);
            
            if (Mathf.Abs(det) < 0.0001f) 
                return p1.y;

            float w1 = ((p2.z - p3.z) * (targetPos.x - p3.x) + (p3.x - p2.x) * (targetPos.y - p3.z)) / det;
            float w2 = ((p3.z - p1.z) * (targetPos.x - p3.x) + (p1.x - p3.x) * (targetPos.y - p3.z)) / det;
            float w3 = 1.0f - w1 - w2;
            
            return w1 * p1.y + w2 * p2.y + w3 * p3.y;
        }
    }
}