using UnityEngine;

public static class LeadingShot {
    public static bool TryGetInterceptDirection(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVel,
        float projectileSpeedXZ, // скорость в плоскости XZ
        float gravity, // положительное число
        out Vector3 direction, out float verticalSpeed, // сюда вернём рассчитанный v_y
        out float timeToHit) // сюда вернём время перехвата
    {
        direction = Vector3.zero;
        verticalSpeed = 0f;
        timeToHit = 0f;

        // --- Решаем по XZ ---
        Vector2 rXZ = new Vector2(targetPos.x - shooterPos.x, targetPos.z - shooterPos.z);
        Vector2 vXZ = new Vector2(targetVel.x, targetVel.z);

        float vSq = vXZ.sqrMagnitude;
        float sSq = projectileSpeedXZ * projectileSpeedXZ;

        float a = vSq - sSq;
        float b = 2f * Vector2.Dot(rXZ, vXZ);
        float c = rXZ.sqrMagnitude;

        float t = SolveEquation(a, b, c);
        if (float.IsPositiveInfinity(t)) {
            return false; // по XZ не можем перехватить
        }

        // --- Подбираем вертикальную скорость ---
        float targetY = targetPos.y + targetVel.y * t;
        verticalSpeed = (targetY - shooterPos.y) / t + 0.5f * gravity * t;

        // --- Собираем итоговый вектор направления ---
        Vector3 aimPoint = targetPos + targetVel * t;
        aimPoint.y = targetY;
        direction = (aimPoint - shooterPos).normalized;
        timeToHit = t;
        return true;
    }

    private static float SolveEquation(float a, float b, float c) {
        float t = float.PositiveInfinity;
        const float EPS = 1e-6f;

        if (Mathf.Abs(a) < EPS) {
            if (Mathf.Abs(b) > EPS) {
                float tCandidate = -c / b;
                if (tCandidate > 0f) {
                    t = tCandidate;
                }
            }
        } else {
            float disc = b * b - 4f * a * c;
            if (disc >= 0f) {
                float sqrtDisc = Mathf.Sqrt(disc);
                float t1 = (-b - sqrtDisc) / (2f * a);
                float t2 = (-b + sqrtDisc) / (2f * a);

                if (t1 > 0f && t2 > 0f) {
                    t = Mathf.Min(t1, t2);
                } else if (t1 > 0f) {
                    t = t1;
                } else if (t2 > 0f) {
                    t = t2;
                }
            }
        }

        return t;
    }
}