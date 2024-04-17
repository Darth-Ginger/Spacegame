
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Spacegame.Managers
{
	public class GameManager : SingletonManager<GameManager>
	{
		public string  Seed { get; private set; } = "Spacegame";
        public int SeedHash => Hash128.Compute(Seed).GetHashCode();
		
        public GameManager(string seed)
        {
            Seed = seed;
        }

		protected override void Awake()
		{
			base.Awake();
			Random.InitState(SeedHash);
		}







    }
	
}