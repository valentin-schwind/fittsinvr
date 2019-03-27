﻿// Version 2.3.3
// ©2013 Reindeer Games
// All rights reserved
// Redistribution of source code without permission not allowed

namespace PrimitivesPro.CSG
{
    public class VertexHash
    {
//        private readonly float bucketSize;
        private readonly CSGVertex[] buckets;
        private readonly float bucketSize2;
        private int count;

        public VertexHash(float bucketSize, int allocSize)
        {
//            this.bucketSize = bucketSize;
            this.bucketSize2 = bucketSize*bucketSize;
            buckets = new CSGVertex[allocSize];
            count = 0;
        }

        public int Hash(CSGVertex p)
        {
            for (var i=0; i<count; i++)
            {
                var item = buckets[i];

                var diffX = p.P.x - item.P.x;
                var diffY = p.P.y - item.P.y;
                var diffZ = p.P.z - item.P.z;
                var sqrMag = diffX*diffX + diffY*diffY + diffZ*diffZ;

                if (sqrMag < bucketSize2)
                {
                    diffX = p.N.x - item.N.x;
                    diffY = p.N.y - item.N.y;
                    diffZ = p.N.z - item.N.z;
                    sqrMag = diffX * diffX + diffY * diffY + diffZ * diffZ;

                    if (sqrMag < bucketSize2)
                    {
                        diffX = p.UV.x - item.UV.x;
                        diffY = p.UV.y - item.UV.y;
                        sqrMag = diffX*diffX + diffY*diffY;

                        if (sqrMag < bucketSize2)
                        {
                            return i;
                        }
                    }
                }
            }

            buckets[count++] = p;
            return count-1;
        }
    }
}
