using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public enum SurfaceMode
    {
        TextureColor, Image, Mixed
    }

    public enum FittingMode
    {
        None, Plane, Cad
    }

    public enum MouseMode
    {
        Rotate, Move, MoveCad, RotateCad
    }

    public enum SubView
    {
        None, Profile
    }

    public enum ProjectionType
    {
        Ortho, Perspective
    }
    public enum ObjName
    {
        Left, Right
    }

}
