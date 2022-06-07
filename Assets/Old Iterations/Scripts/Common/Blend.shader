Shader "Custom/Simple Additive" {
    Properties{
        _MainTex("Texture to blend", 2D) = "black" {}
    }
        SubShader{
            Tags { "Queue" = "Transparent" }
            Pass {
                //Blend Off
                Blend OneMinusSrcColor One
                SetTexture[_MainTex] { combine texture }
            }
            /*Pass{
                Blend One One
                SetTexture[_MainTex] { combine texture }
            }*/
    }
}