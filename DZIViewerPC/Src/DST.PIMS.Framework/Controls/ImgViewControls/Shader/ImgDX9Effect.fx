float B :register(C0);
float G :register(C1);
float R :register(C2);

float Brightness:register(C4);
float Contrast:register(C5);
float gama:register(C6);

sampler Texture1Sampler : register(s0);

float4 main(float2 texcoord : TEXCOORD) : COLOR
{
    float4 color;
    float4 temp0, temp1, temp2;
    float4 temp3;
    // Def c5
    // Dcl r0.x a0.xy
    // Dcl v0.x s0
    // Tex r0 t0 s0
    temp0 = tex2D(Texture1Sampler, texcoord);
    // Rcp r1.w c7.x
    temp1.w = 1 / gama.x;
    // Mov r1.y c5.y
    temp1.y = 0;
    // Max r2.w c1.x r1.y
    temp2.w = max(Contrast.x, temp1.y);
    // Rcp r1.x r0.w
    temp1.x = 1 / temp0.w;
    // Mad r1.xyz r0 r1.x c5.x
    temp1.xyz = temp0 * temp1.x + -0.5;
    // Mad r1.xyz r1 r2.w c0.x
    temp1.xyz = temp1 * temp2.www + Brightness.x;
    // Add r1.x r1.x c2.x
    temp1.x = temp1.x + R.x;
    // Add r1.x r1.x c5.z
    temp1.x = temp1.x + 0.5;
    // Max r2.x r1.x c5.y
    temp2.x = max(temp1.x, 0);
    // Log r2.x r2.x
    temp2.x = log2(temp2.x);
    // Add r2.w r1.y c3.x
    temp2.w = temp1.y + G.x;
    // Add r1.x r1.z c4.x
    temp1.x = temp1.z + B.x;
    // Add r1.x r1.x c5.z
    temp1.x = temp1.x + 0.5;
    // Max r3.w r1.x c5.y
    temp3.w = max(temp1.x, 0);
    // Log r2.z r3.w
    temp2.z = log2(temp3.w);
    // Add r2.w r2.w c5.z
    temp2.w = temp2.w + 0.5;
    // Max r1.x r2.w c5.y
    temp1.x = max(temp2.w, 0);
    // Log r2.y r1.x
    temp2.y = log2(temp1.x);
    // Mul r1.xyz r1.w r2
    temp1.xyz = temp1.www * temp2;
    // Exp r2.x r1.x
    temp2.x = exp2(temp1.x);
    // Exp r2.y r1.y
    temp2.y = exp2(temp1.y);
    // Exp r2.z r1.z
    temp2.z = exp2(temp1.z);
    // Mul r1.xyz r0.w r2
    temp1.xyz = temp0.www * temp2;
    // Min r0.x r1.x c5.w
    temp0.x = min(temp1.x, 1);
    // Min r0.y r1.y c5.w
    temp0.y = min(temp1.y, 1);
    // Min r0.z r1.z c5.w
    temp0.z = min(temp1.z, 1);
    // Mov oC0 r0
    color = temp0;
    // End

    return color;
}
