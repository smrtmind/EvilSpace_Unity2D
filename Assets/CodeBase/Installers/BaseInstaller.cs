using CodeBase.Effects;
using CodeBase.Player;
using CodeBase.Service;
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

    public override void InstallBindings()
    {
        BindPlayer();
        BindParticlePool();
        BindScreenBounds();
        BindCameraController();
        BindTouchController();
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
}