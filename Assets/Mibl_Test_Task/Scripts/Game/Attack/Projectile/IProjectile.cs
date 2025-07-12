using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Attack.Projectile
{
    public interface IProjectile
    {
        void Launch(LayerMask targetLayerMask, Vector2 direction, float speed);
        void ReturnToPool();
        ProjectileType Type { get; }
    }
}