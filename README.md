# urp-ui-blur-dev

Demo and development project of [URP UI Blur](https://github.com/RottenEagle1337/urp-ui-blur), a material based
frosted glass blur for uGUI in URP. The package is included as a git submodule.

**To use the blur in your project, install the package; you do not need this project.**

| Repository | Path here | Contents |
|---|---|---|
| [urp-ui-blur](https://github.com/RottenEagle1337/urp-ui-blur) | `Packages/com.rotteneagle.urp-ui-blur` | Unity package |
| **urp-ui-blur-dev** | | this Unity project |

<img src="Packages/com.rotteneagle.urp-ui-blur/Documentation~/images/hero.gif" width="640" alt="Showcase scene">

## Getting started

```
git clone --recursive https://github.com/RottenEagle1337/urp-ui-blur-dev.git
```

Already cloned without `--recursive`: `git submodule update --init`.

- Unity 6000.6 (see `ProjectSettings/ProjectVersion.txt`), URP 17.6, Input System, linear color space.
- One pipeline asset, `Assets/Settings/UniversalRP.asset`, with one renderer that has only the **Ui Blur Feature**
  (the `UI` layer is excluded from its Transparent Layer Mask).

## Scenes

All in `Assets/Scenes`, in the build settings in this order.

| Scene | Shows | Controls |
|---|---|---|
| **Showcase** | game HUD, pause menu and settings popup in one canvas; the menu blur fades with the menu | `Esc` menu, mouse for buttons and the strength slider |
| **Basics** | three panels with `_BlurStrength` 0.3 / 0.6 / 1.0 over moving UI | — |
| **BlurTest** | test stand: overlapping panels, RectMask2D, stencil Mask, moving panels, halo probe, stress test | `1` menu, `Q`/`A` strength, `Space` animate, `M` motion, `P`/`O` stress panels, `H` HUD |

## License

[Unlicense](LICENSE.md) (public domain).
