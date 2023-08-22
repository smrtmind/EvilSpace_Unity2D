using System.Linq;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    [CreateAssetMenu(fileName = "PlanetStorage", menuName = "ScriptableObjects/PlanetStorage")]
    public class PlanetStorage : ScriptableObject
    {
        [SerializeField] private Planet[] planets;

        public PlanetType GetRandomPlanetType() => planets[Random.Range(0, planets.Length)].Type;

        public Planet GetPlanet(PlanetType type) => planets.FirstOrDefault(planet => planet.Type == type);
    }
}
