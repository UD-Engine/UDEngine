# UDEngine Unity Bullet Hell Engine

## Introduction  
Built on __Unity3D__ and __DOTween/LeanTween__, UDEngine aims to be a minimal, extensible, bullet-hell game engine, that features wide application and customization using callbacks as its own core. 

## Notice
For developmental reasons, the submodules of `UDEngine` is linked with `git@github.com:UD-Engine/SUBMODULE_NAME.git`, which is SSH-key based. This means only directory owners could have the access to directly clone these submodules. To circumvent this problem, __PLEASE remove all submodules after cloning the main repository, and THEN manually, following the structure in the Github Repo, clone all submodules using `http://github.com/UD-Engine/SUBMODULE_NAME.git` respectively__. I feel sorry for this trouble, but at this stage of development, such configuration is necessary for me to push everything up to date easily.  

## Current State
The implementation of `UDEngine.Core` namespace is basically __complete__. This means that the core, game-style independent features are all now available.  

Work will be put into the files inside of the `Engine/Commons` directory AND `Engine/Plugin`.  

`Engine/Commons` defines common utilities, that may be game-style independent, yet not so essential enough. Scripts in this folder are all under `UDEngine.Commons` namespace. They are supposed to be oblivious of the Core implementation, and that will be developed incrementally.

`Engine/Plugin` is home to many plugins that makes the engine itself much more suitable for games of specific styles, such as Touhou. For example, we provide `ShootDust` (shoot origin dust-like effects) and `PatternGenerator` (generating shooter patterns on the fly) implementations. All scripts in this folder are supposed to be under `UDEngine.Plugin` namespace, and oblivious of the `UDEngine.Core` and `UDEngine.Commons` implementation.

Examples of the engine usage is still very few at this stage. However, new examples would be added to `Example/Sample` directory. You can use scenes inside of them as the boilerplate, as certain necessary objects are hard to configure. 

API reference of the `UDEngine.Core` is under work. You will find the link at the description of Github Repository. 

The engine uses one or two awesome and useful plugins, which are NOT written by me. You can find the in the `Plugins` folder. They will all be licenses as according to their own READMEs.  

I truly welcome people to come and test the engine, and hopefully contribute to the development.  

## Benchmark
Under my Macbook Pro (Early 2015) environment, with an 8 GB RAM, a 2.7 GHz Intel Core i5 CPU, and an Intel Iris Graphics 6100 1536 MB GPU (a very mediocre one, incapable of running *Portal*), the engine runs, after game build, at 60 FPS with max 2000~2300 bullets moving with certain easetype, recycling, and collision detecting. So usually, performance should NOT be troubling. (I have tried my best to use Profiler for careful optimization)  

## Showcase and Update  
### Sept. 12
![1.gif](/Showcase/1.gif)
