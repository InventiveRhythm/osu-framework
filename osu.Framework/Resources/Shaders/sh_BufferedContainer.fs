#ifndef BUFFERED_CONTAINER_FS
#define BUFFERED_CONTAINER_FS

#include "sh_Utils.h"
#include "sh_Masking.h"
#include "sh_TextureWrapping.h"
#include "sh_Blending.h"

layout(location = 2) in mediump vec2 v_TexCoord;

layout(set = 0, binding = 0) uniform lowp texture2D m_Texture;
layout(set = 0, binding = 1) uniform lowp sampler m_Sampler;

layout(location = 0) out vec4 o_Colour;

void main(void) 
{
    vec2 wrappedCoord = wrap(v_TexCoord, v_TexRect);
    vec4 wrapped = wrappedSampler(wrappedCoord, v_TexRect, m_Texture, m_Sampler, -0.9);
    vec4 roundedColor = getRoundedColor(wrapped, wrappedCoord);

    float originalAlpha = wrapped.a * v_Colour.a;
    if (originalAlpha > 0.0)
    {
        float mask = roundedColor.a / originalAlpha;
        roundedColor.rgb *= mask;
    }

    o_Colour = applyBlendColourMode(roundedColor);
}

#endif