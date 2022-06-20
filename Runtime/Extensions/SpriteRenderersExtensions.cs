using System.Collections.Generic;
using UnityEngine;

namespace DesertImage
{
    public static class SpriteRenderersExtensions
    {
        public static void SetSprite(this IEnumerable<SpriteRenderer> objects, Sprite sprite)
        {
            if (objects == null) return;

            foreach (var obj in objects)
            {
                if (!obj) continue;

                obj.sprite = sprite;
            }
        }

        public static Vector2 GetTextureSpaceCoord(this SpriteRenderer spriteRenderer, Vector3 worldPos)
        {
            var sprite = spriteRenderer.sprite;

            var ppu = sprite.pixelsPerUnit;

            // Local position on the sprite in pixels.
            Vector2 localPos = spriteRenderer.transform.InverseTransformPoint(worldPos) * ppu;

            // When the sprite is part of an atlas, the rect defines its offset on the texture.
            // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
            var texSpacePivot = new Vector2(sprite.rect.x, sprite.rect.y) + sprite.pivot;

            var texSpaceCoord = texSpacePivot + localPos;

            return texSpaceCoord;
        }

        public static Vector2 GetTextureSpaceUV(this SpriteRenderer spriteRenderer, Vector3 worldPos)
        {
            var sprite = spriteRenderer.sprite;

            var tex = sprite.texture;
            var texSpaceCoord = spriteRenderer.GetTextureSpaceCoord(worldPos);

            // Pixels to UV(0-1) conversion.
            var uvs = texSpaceCoord;
            uvs.x /= tex.width;
            uvs.y /= tex.height;


            return uvs;
        }
    }
}