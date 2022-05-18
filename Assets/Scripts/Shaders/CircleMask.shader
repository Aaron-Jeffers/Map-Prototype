Shader "Custom/CircleMask" {
    SubShader{
        Tags { "Queue" = "Transparent+3" }
        Pass {
          Blend Zero One 
        }
    }
}