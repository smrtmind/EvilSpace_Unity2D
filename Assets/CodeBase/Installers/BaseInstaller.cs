using CodeBase.Effects;
using CodeBase.Player;
using CodeBase.Service;
using CodeBase.UI;
using CodeBase.Utils;
using UnityEngine;
using Zenject;

public class BaseInstaller : MonoInstaller
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ParticlePool particlePool;
    [SerializeField] private ScreenBounds screenBounds;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TouchController touchController;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private UserInterface userInterface;

    public override void InstallBindings()
    {
        BindPlayer();
        BindParticlePool();
        BindScreenBounds();
        BindCameraController();
        BindTouchController();
        BindWeaponController();
        BindUserInterface();
    }

    private void BindPlayer()
    {
        Container.Bind<PlayerController>().FromInstance(playerController).AsSingle().NonLazy();
        Container.QueueForInject(playerController);
    }

    private void BindParticlePool()
    {
        Container.Bind<ParticlePool>().FromInstance(particlePool).AsSingle().NonLazy();
        Container.QueueForInject(particlePool);
    }

    private void BindScreenBounds()
    {
        Container.Bind<ScreenBounds>().FromInstance(screenBounds).AsSingle().NonLazy();
        Container.QueueForInject(screenBounds);
    }

    private void BindCameraController()
    {
        Container.Bind<CameraController>().FromInstance(cameraController).AsSingle().NonLazy();
        Container.QueueForInject(cameraController);
    }

    private void BindTouchController()
    {
        Container.Bind<TouchController>().FromInstance(touchController).AsSingle().NonLazy();
        Container.QueueForInject(touchController);
    }

    private void BindWeaponController()
    {
        Container.Bind<WeaponController>().FromInstance(weaponController).AsSingle().NonLazy();
        Container.QueueForInject(weaponController);
    }

    private void BindUserInterface()
    {
        Container.Bind<UserInterface>().FromInstance(userInterface).AsSingle().NonLazy();
        Container.QueueForInject(userInterface);
    }
}