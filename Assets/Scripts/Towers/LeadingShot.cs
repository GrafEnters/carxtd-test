using UnityEngine;

public static class LeadingShot
{
    // Попытаться получить направление для выстрела, чтобы попасть по движущейся цели.
    // Возвращает true + out direction если найдено решение; иначе false.
    // shooterPos  - позиция пушки S
    // targetPos   - текущая позиция цели P
    // targetVel   - вектор скорости цели V
    // projectileSpeed - модуль скорости снаряда s (положителен)
    public static bool TryGetInterceptDirection(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVel, float projectileSpeed, out Vector3 direction)
    {
        direction = Vector3.zero;

        if (projectileSpeed <= 0f)
        {
            return false;
        }

        Vector3 r = targetPos - shooterPos;
        float rSq = Vector3.SqrMagnitude(r);
        if (rSq < Mathf.Epsilon)
        {
            // Цель уже в позиции пушки — любое направление (например вперёд)
            direction = Vector3.forward;
            return true;
        }

        float vSq = Vector3.SqrMagnitude(targetVel);
        float sSq = projectileSpeed * projectileSpeed;

        float a = vSq - sSq;
        float b = 2f * Vector3.Dot(r, targetVel);
        float c = rSq;

        float t = float.PositiveInfinity;

        if (Mathf.Abs(a) < 1e-6f)
        {
            // Тогда линейное уравнение: b t + c = 0 -> t = -c/b
            if (Mathf.Abs(b) > 1e-6f)
            {
                float tCandidate = -c / b;
                if (tCandidate > 0f)
                {
                    t = tCandidate;
                }
            }
        }
        else
        {
            float disc = b * b - 4f * a * c;
            if (disc >= 0f)
            {
                float sqrtDisc = Mathf.Sqrt(disc);
                float t1 = (-b - sqrtDisc) / (2f * a);
                float t2 = (-b + sqrtDisc) / (2f * a);

                // берем наименьшее положительное t
                if (t1 > 0f && t2 > 0f)
                {
                    t = Mathf.Min(t1, t2);
                }
                else if (t1 > 0f)
                {
                    t = t1;
                }
                else if (t2 > 0f)
                {
                    t = t2;
                }
            }
        }

        if (float.IsPositiveInfinity(t))
        {
            // Нет допустимого положительного решения — перехват невозможен при этой скорости.
            return false;
        }

        Vector3 aimPoint = targetPos + targetVel * t;
        direction = (aimPoint - shooterPos).normalized;
        return true;
    }
}