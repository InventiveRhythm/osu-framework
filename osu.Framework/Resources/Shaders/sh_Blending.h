#ifndef BLENDING_H
#define BLENDING_H

lowp vec4 applyBlendColourMode(lowp vec4 colour)
{
    float isWhite = float(g_BlendColourMode == 2);
    float isDark  = float(g_BlendColourMode == 3 || g_BlendColourMode == 4);

    vec3 target = vec3(isWhite);
    float enabled = isWhite + isDark;

    colour.rgb = mix(colour.rgb, mix(target, colour.rgb, colour.a), enabled);
    return colour;
}

#endif