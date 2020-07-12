

v2f vert (appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

    v.tangent.xyz = normalize(v.tangent.xyz);
    v.normal = normalize(v.normal);

    TANGENT_SPACE_ROTATION;
    o.TtoV0 = normalize(mul(rotation, UNITY_MATRIX_IT_MV[0].xyz));
    o.TtoV1 = normalize(mul(rotation, UNITY_MATRIX_IT_MV[1].xyz));

    o.worldView = normalize(WorldSpaceViewDir(v.vertex));
    o.worldNormal = UnityObjectToWorldNormal(v.normal);

    o.color = v.color;

    return o;
}

fixed4 frag (v2f i) : SV_Target
{
    fixed4 colorControl = i.color;
    fixed4 fragSmp = tex2D(_MainTex, i.uv);
    fixed alphaInTex = fragSmp.a;
    fixed4 col = fragSmp * _Color;

    fixed3 tNormal = fixed3(0,0,1);
    fixed2 vN;
    vN.x = dot(i.TtoV0, tNormal);
    vN.y = dot(i.TtoV1, tNormal);

    fixed3 tNormal = fixed3(0, 0, 1);
    finalColor += col * tex2D(_MatCapTex01, vN * 0.5 + 0.5) * colorControl.r * _Scale01;
    #ifdef _MatCap02
    finalColor += col * tex2D(_MapCapTex02, vN * 0.5 + 0.5) * colorControl.g * _Scale02;
    #ifdef _MatCap03
    finalColor += col * tex2D(_MapCapTex03, vN * 0.5 + 0.5) * colorControl.b * _Scale03;
    #ifdef _MatCap04
    finalColor += col * tex2D(_MapCapTex02, vN * 0.5 + 0.5) * colorControl.a * _Scale04;
    #endif
    // Ambient color
    finalColor += col * _AmbieColor;
    finalColor.rgb += power(1.0 - saturate(dot(i.worldNormal, i.worldView)), _RimPower) * _RimColor.rgb * _RimScale;

    float indicatedFactor = any(_IndicatedAlpha) ? _IndicatedAlpha : 1;
    finalColor = fixed4(finalColor.rgb, indicatedFactor * alphaInTex);

    return finalColor;
}