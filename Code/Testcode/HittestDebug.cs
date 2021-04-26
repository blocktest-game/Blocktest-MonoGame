using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Blocktest
{
    struct Debug
    {
        public static Point hitDebugPoint = new(0);
        public static Rectangle hitDebugRect;
        public static void HittestDebug()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                hitDebugPoint.X = Mouse.GetState().Position.X;
                hitDebugPoint.Y = Mouse.GetState().Position.Y;
            }

            //hitDebugPoint.X = 1;
            //hitDebugPoint.Y = 2;

            hitDebugRect.X = hitDebugPoint.X;
            hitDebugRect.Y = hitDebugPoint.Y;

            hitDebugRect.Width = Mouse.GetState().Position.X - hitDebugPoint.X;
            hitDebugRect.Height = Mouse.GetState().Position.Y - hitDebugPoint.Y;

            Point hitDebugEnd;
            hitDebugEnd.X = Mouse.GetState().Position.X;
            hitDebugEnd.Y = Mouse.GetState().Position.Y;

            //hitDebugEnd.X = 2;
            //hitDebugEnd.Y = 3;

            bool hitDetect = false;

            Point contactPoint;
            Point contactNormal;
            float nearContactTime;

            foreach (Tile tile in Globals.ForegroundTilemap.allTiles) {
                if (HitTests.RayToRectangle(hitDebugPoint, hitDebugEnd, tile.rectangle, out contactPoint, out contactNormal, out nearContactTime)) {
                    hitDetect = true;
                }
            }

            /*foreach (Tile tile in Globals.ForegroundTilemap.allTiles) {
                if (HitTests.RectangleToRectangle(hitDebugRect, tile.rectangle)) {
                    hitDetect = true;
                }
            }*/

            /*foreach (Tile tile in Globals.ForegroundTilemap.allTiles) {
                if (HitTests.PointToRectangle(Mouse.GetState().Position, tile.rectangle)) {
                    hitDetect = true;
                }
            }*/

            if (hitDetect == true) {
                Mouse.SetCursor(MouseCursor.Crosshair);
            } else {
                Mouse.SetCursor(MouseCursor.Arrow);
            }
        }
    }
}