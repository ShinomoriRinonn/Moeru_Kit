#if !defined(FLOW_INCLUDED)
#define FLOW_INCLUDED

float2 FlowUV (float2 uv, float time) {
	return uv + time;
}

// 获得锯齿波
float2 FlowUVVector (float2 uv, float2 flowVector, float time) {
	return uv - flowVector * frac(time);
}

// 获得三角波
float3 FlowUVW (float2 uv, float2 flowVector, float time) {
	float progress = frac(time);
	float3 uvw;
	uvw.xy = uv - flowVector * progress;
	uvw.z = 1 - abs(1 - 2 * progress);
	return uvw;
}

float3 FlowUVWDouble (float2 uv, float2 flowVector, float time, bool flowB) {
	float phaseOffset = flowB ? 0.5 : 0;
	float progress = frac(time + phaseOffset);
	float3 uvw;
	uvw.xy = uv - flowVector * progress;
	uvw.z = 1 - abs(1 - 2 * progress);
	return uvw;
}



#endif