﻿using System;
using Labyrinth.Infrastructure.GameStateSystem;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game
{
	public class GameSceneInstaller : MonoInstaller
	{
		[SerializeField] private Canvas _inputCanvasPrefab;
		
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<SaveManager>().AsSingle();
			Container.BindInterfacesTo<AttemptTracker>().AsSingle();
			Container.BindInterfacesTo<GameStateManager>().AsSingle();
			
			Container.Bind<Joystick>().FromMethod(InstantiateInputCanvas).AsSingle();
		}

		private Joystick InstantiateInputCanvas()
		{
			return Instantiate(_inputCanvasPrefab).GetComponentInChildren<Joystick>();
		}
	}
}