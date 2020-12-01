using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using Liquid.Objects;
using Liquid.Objects.Structs;
using static System.Math;
using static Liquid.Objects.GlobalLists;

namespace Liquid.Hacks
{
    static class Aimbot
    {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetAsyncKeyState(
            int vKey
        );

        public static void TriggerThread()
        {
            while (Globals.active)
            {
                if (!Globals.TriggerEnabled && !Globals.AimShootOnCollide)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }

                if (Globals.TriggerPressOnlyEnabled && !Globals.AimShootOnCollide)
                {
                    if ((GetAsyncKeyState(Globals.TriggerKey) & 0x8000) > 0)
                    {
                        if (CBasePlayer.CrosshairID > 0 && CBasePlayer.CrosshairID < EngineDLL.MaxPlayer + 2)
                        {
                            CBaseEntity baseEntity = entityList[CBasePlayer.CrosshairID - 1];
                            if (baseEntity == null) continue;
                            CCSPlayer crossEntity = new CCSPlayer(baseEntity);
                            if (crossEntity == null) continue; // TRIGGER BOT CRASH FIX
                            if (crossEntity != null && 
                                crossEntity.Team != CBasePlayer.Team ||
                                Globals.FriendlyFire)
                            {
                                Thread.Sleep(1);
                                ClientDLL.ForceAttack(true);
                                Thread.Sleep(5);
                                ClientDLL.ForceAttack(false);
                            }
                        }
                    }
                }
                else
                {
                    if (CBasePlayer.CrosshairID > 0 && CBasePlayer.CrosshairID < EngineDLL.MaxPlayer + 2)
                    {
                        CBaseEntity baseEntity = entityList[CBasePlayer.CrosshairID - 1];
                        if (baseEntity == null) continue;
                        CCSPlayer crossEntity = new CCSPlayer(baseEntity);
                        if (crossEntity == null) continue;
                        if (crossEntity != null && crossEntity.Team != CBasePlayer.Team)
                        {
                            Thread.Sleep(1);
                            ClientDLL.ForceAttack(true);
                            Thread.Sleep(5);
                            ClientDLL.ForceAttack(false);
                        }
                    }
                }

                Thread.Sleep(2);
            }
        }

        public static void SearchFriend()
        {
            while (Globals.searchFriend) 
            {
                int mp = EngineDLL.MaxPlayer;
                Rectangle screen = Objects.Structs.Misc.GetWindowRect();
                Vector2 screenOrigin = new Vector2(screen.Width / 2, screen.Height / 2);
                double latestDistance = screen.Width;
                for (int i = 0; i < mp; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);
                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;
                    if (Globals.MyFriend != null)
                        if (entity == Globals.MyFriend) continue;

                    Vector3 entSelectedPos = entity.GetBonePosition(GetHitbox());
                    Vector2 entPosOnScreen;
                    if (entSelectedPos.PointOnScreen(out entPosOnScreen))
                    {
                        if (entPosOnScreen.x > screen.Width || entPosOnScreen.x < 0 || entPosOnScreen.y > screen.Height || entPosOnScreen.y < 0)
                        {
                            continue;
                        }
                    }
                    else continue;

                    double dist = Sqrt(Pow(screenOrigin.x - entPosOnScreen.x, 2) + Pow(screenOrigin.y - entPosOnScreen.y, 2));
                    if (dist < latestDistance)
                    {
                        latestDistance = dist;
                        MessageBox.Show(entity.index+"");
                        Globals.MyFriend = entity;
                    }
                }
            }
        }

        public static void AimbotThread()
        {
            while (true)
            {
                if (!Globals.AimEnabled)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }

                int mp = EngineDLL.MaxPlayer;
                Rectangle screen = Objects.Structs.Misc.GetWindowRect();
                Vector2 screenOrigin = new Vector2(screen.Width / 2, screen.Height / 2);
                double latestDistance = screen.Width;
                Vector3 closestEntityPos = new Vector3(99999f, 0f, 0f);
                for (int i = 0; i < mp; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);
                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;
                    if (entity.Team == CBasePlayer.Team && !Globals.FriendlyFire) continue;
                    if (Globals.MyFriend != null)
                        if (entity == Globals.MyFriend) continue;
                    Vector3 entSelectedPos = entity.GetBonePosition(GetHitbox());
                    Vector2 entPosOnScreen;
                    if (entSelectedPos.PointOnScreen(out entPosOnScreen))
                    {
                            if (entPosOnScreen.x > screen.Width || entPosOnScreen.x < 0 || entPosOnScreen.y > screen.Height || entPosOnScreen.y < 0)
                            {
                                continue;
                            }

                    }
                    else continue;

                    double dist = Sqrt(Pow(screenOrigin.x - entPosOnScreen.x, 2) + Pow(screenOrigin.y - entPosOnScreen.y, 2));
                    if (dist < latestDistance)
                    {
                        latestDistance = dist;
                        closestEntityPos = entSelectedPos;
                    }
                }

                if (closestEntityPos.x != 99999999 && (GetAsyncKeyState(Globals.TriggerKey) & 0x8000) > 0)
                {
                    Angle AimAt = CalcAngle(CBasePlayer.VectorEyeLevel, closestEntityPos);


                    if (Math.Abs(AimAt.x) > Globals.AimFlick) continue;


                    if (Globals.AimRecoil)
                    {
                       Angle Punch = CBasePlayer.ViewPunchAngle * 2.0f;
                        AimAt.x -= Punch.x;
                        AimAt.y -= Punch.y;
                    }

                    CBasePlayer.ViewAngle = AimAt;

                    if (!Globals.AimShootOnCollide)
                    {
                        if (weaponList.ActiveWeapon.IsSniper())
                        {
                            ClientDLL.ForceRightAttack(true);
                            ClientDLL.ForceAttack(true);
                            ClientDLL.ForceRightAttack(false);
                            ClientDLL.ForceAttack(false);
                        }
                        else
                        {
                            ClientDLL.ForceAttack(true);
                            ClientDLL.ForceAttack(false);
                        }
                    }
                }

                Thread.Sleep(Globals.UsageDelay);
            }
        }

        public static void AimAssistThread()
        {
            while (true)
            {
                if (!Globals.AimAssist)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }
                if (!EngineDLL.InGame)
                {
                    Thread.Sleep(Globals.IdleWait);
                    continue;
                }

                int mp = EngineDLL.MaxPlayer;
                Rectangle screen = Objects.Structs.Misc.GetWindowRect();
                Vector2 screenOrigin = new Vector2(screen.Width / 2, screen.Height / 2);
                double latestDistance = screen.Width;
                Vector3 closestEntityPos = new Vector3(99999f, 0f, 0f);

                for (int i = 0; i < mp; i++)
                {
                    CBaseEntity baseEntity = entityList[i];
                    if (baseEntity == null) continue;
                    CCSPlayer entity = new CCSPlayer(baseEntity);
                    if (entity == null) continue;
                    if (entity.Dormant) continue;
                    if (entity.Health <= 0) continue;
                    if (entity.Team == CBasePlayer.Team) continue;

                    Vector3 entSelectedPos = entity.GetBonePosition(0);
                    Vector2 entPosOnScreen;
                    if (entSelectedPos.PointOnScreen(out entPosOnScreen))
                    {
                        MessageBox.Show(entPosOnScreen.x + "");
                        if (entPosOnScreen.x > screen.Width || entPosOnScreen.x < 0 || entPosOnScreen.y > screen.Height || entPosOnScreen.y < 0)
                        {
                            continue;
                        }
                    }
                    else continue;

                    double dist = Sqrt(Pow(screenOrigin.x - entPosOnScreen.x, 2) + Pow(screenOrigin.y - entPosOnScreen.y, 2));
                    if (dist < latestDistance)
                    {
                        latestDistance = dist;
                        closestEntityPos = entSelectedPos;
                    }
                }

                if (closestEntityPos.x < 200f)
                {
                    Angle AimAt = CalcSmoothAngle(CBasePlayer.VectorEyeLevel, closestEntityPos);

                    CBasePlayer.ViewAngle = AimAt;
                    Thread.Sleep(1000);

                }
            }
        }

        static Angle CalcAngle(Vector3 src, Vector3 dst)
        {
            
            Angle angles;
            Vector3 e = new Vector3(dst.x - src.x, dst.y - src.y, dst.z - src.z);
            float eh = (float)Sqrt(e.x * e.x + e.y * e.y);

            angles.x = (float)(Atan2(-e.z, eh) * 180 / PI);
            angles.y = (float)(Atan2(e.y, e.x) * 180 / PI);

            return angles;
        }

        static Angle CalcSmoothAngle(Vector3 src, Vector3 dst)
        {
            dst.x = (float)(((double)src.x + (double)dst.x)/3);
            dst.y = (float)(((double)src.y + (double)dst.y)/ 3);
            dst.z = (float)(((double)src.z + (double)dst.z)/ 3);

            return CalcAngle(src,dst);
        }

        private static int GetHitbox()
        {
            return (int)Globals.AimPosition[0];
            Random random = new Random();
            int num = random.Next(0, 100);
            return (int)Globals.AimPosition[num > Globals.HeadShotPercentage ? 1 : 0];
        }
    }
}
