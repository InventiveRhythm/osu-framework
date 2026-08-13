#ifndef TEXTURE_FS
#define TEXTURE_FS

#include "sh_Utils.h"
#include "sh_Masking.h"
#include "sh_TextureWrapping.h"

layout(location = 2) in mediump vec2 v_TexCoord;

layout(set = 0, binding = 0) uniform lowp texture2D m_Texture;
layout(set = 0, binding = 1) uniform lowp sampler m_Sampler;

layout(location = 0) out vec4 o_Colour;

void main(void)
{
    vec4 col = texture(sampler2D(m_Texture, m_Sampler), v_TexCoord);
    o_Colour = col;
}

#endif