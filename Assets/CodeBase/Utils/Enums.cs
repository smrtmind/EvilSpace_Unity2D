namespace CodeBase.Utils
{
    public static class Enums
    {
        public enum WeaponType
        {
            None,
            Blaster,
            MachineGun,
            EnemyWeapon
        }

        public enum EnemyType
        {
            None,
            Asteroid,
            SmallShip,
            MediumShip,
            LargeShip,
            Boss
        }

        public enum MovementState
        {
            Moveless,
            Left,
            Right
        }

        public enum ParticleType
        {
            None,
            AsteroidExplosion,
            SmallShipExplosion,
            MediumShipExplosion,
            LargeShipExplosion,
            BossExplosion,
            SparksHit,
            PlayerExplosion
        }
    }
}
