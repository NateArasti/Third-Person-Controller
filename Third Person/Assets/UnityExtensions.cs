using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UnityExtensions
{
    private static Camera _mainCamera;
    ///<summary>
    /// Stored Main Camera
    ///</summary>
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary =
        new Dictionary<float, WaitForSeconds>();
    ///<summary>
    /// Non-allocating WaitForSeconds
    ///</summary>
    public static WaitForSeconds Wait(float waitTime)
    {
        if (WaitDictionary.TryGetValue(waitTime, out var waitForSeconds))
        {
            return waitForSeconds;
        }

        WaitDictionary[waitTime] = new WaitForSeconds(waitTime);
        return WaitDictionary[waitTime];
    }

    ///<summary>
    ///World Position of Canvas element
    ///</summary>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform rectTransform)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            rectTransform.position,
            MainCamera,
            out var result);
        return result;
    }

    ///<summary>
    ///Destroy all kids of object
    ///</summary>
    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform childTransform in transform)
        {
            Object.Destroy(childTransform.gameObject);
        }
    }

    ///<summary>
    ///Change alpha of Sprites to create Fade effect
    ///</summary>
    public static void Fade(this SpriteRenderer spriteRenderer, float alpha)
    {
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    ///<summary>
    /// Gets random object from list
    ///</summary>
    public static T GetRandomObject<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new UnityException("Can't get random object from empty list");
        return list[Random.Range(0, list.Count)];
    }

    ///<summary>
    /// Remove random object from list
    ///</summary>
    public static void RemoveRandomObject<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new UnityException("Can't remove random object from empty list");
        list.RemoveAt(Random.Range(0, list.Count));
    }

    ///<summary>
    /// Call Action for each object in collection
    ///</summary>
    public static void ForEachAction<T>(this ICollection<T> collection, UnityAction<T> action)
    {
        foreach (var obj in collection)
        {
            action.Invoke(obj);
        }
    }

    ///<summary>
    /// Call Action after delay
    ///</summary>
    public static IEnumerator InvokeAfterDelayCoroutine(System.Action action, float time)
    {
        yield return Wait(time);
        action.Invoke();
    }
}
