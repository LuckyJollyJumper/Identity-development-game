using UnityEngine;
using UnityEngine.UI;

public class CustomVerticalLayout : VerticalLayoutGroup
{
    [SerializeField] bool LastControlHeight;
    
    // protected void SetChildrenAlongAxis(int axis, bool isVertical)
    //     {
    //         float size = rectTransform.rect.size[axis];
    //         bool controlSize = (axis == 0 ? m_ChildControlWidth : m_ChildControlHeight);
    //         bool useScale = (axis == 0 ? m_ChildScaleWidth : m_ChildScaleHeight);
    //         bool childForceExpandSize = (axis == 0 ? m_ChildForceExpandWidth : m_ChildForceExpandHeight);
    //         float alignmentOnAxis = GetAlignmentOnAxis(axis);

    //         bool alongOtherAxis = (isVertical ^ (axis == 1));
    //         int startIndex = m_ReverseArrangement ? rectChildren.Count - 1 : 0;
    //         int endIndex = m_ReverseArrangement ? 0 : rectChildren.Count;
    //         int increment = m_ReverseArrangement ? -1 : 1;
    //         if (alongOtherAxis)
    //         {
    //             float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);

    //             for (int i = startIndex; m_ReverseArrangement ? i >= endIndex : i < endIndex; i += increment)
    //             {
    //                 RectTransform child = rectChildren[i];
    //                 float min, preferred, flexible;
    //                 base.GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
    //                 float scaleFactor = useScale ? child.localScale[axis] : 1f;

    //                 float requiredSpace = Mathf.Clamp(innerSize, min, flexible > 0 ? size : preferred);
    //                 float startOffset = GetStartOffset(axis, requiredSpace * scaleFactor);
    //                 if (controlSize)
    //                 {
    //                     SetChildAlongAxisWithScale(child, axis, startOffset, requiredSpace, scaleFactor);
    //                 }
    //                 else
    //                 {
    //                     float offsetInCell = (requiredSpace - child.sizeDelta[axis]) * alignmentOnAxis;
    //                     SetChildAlongAxisWithScale(child, axis, startOffset + offsetInCell, scaleFactor);
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             float pos = (axis == 0 ? padding.left : padding.top);
    //             float itemFlexibleMultiplier = 0;
    //             float surplusSpace = size - GetTotalPreferredSize(axis);

    //             if (surplusSpace > 0)
    //             {
    //                 if (GetTotalFlexibleSize(axis) == 0)
    //                     pos = GetStartOffset(axis, GetTotalPreferredSize(axis) - (axis == 0 ? padding.horizontal : padding.vertical));
    //                 else if (GetTotalFlexibleSize(axis) > 0)
    //                     itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
    //             }

    //             float minMaxLerp = 0;
    //             if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
    //                 minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));

    //             for (int i = startIndex; m_ReverseArrangement ? i >= endIndex : i < endIndex; i += increment)
    //             {
    //                 RectTransform child = rectChildren[i];
    //                 float min, preferred, flexible;
    //                 base.GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
    //                 float scaleFactor = useScale ? child.localScale[axis] : 1f;

    //                 float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
    //                 childSize += flexible * itemFlexibleMultiplier;
    //                 if (controlSize)
    //                 {
    //                     SetChildAlongAxisWithScale(child, axis, pos, childSize, scaleFactor);
    //                 }
    //                 else
    //                 {
    //                     float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
    //                     SetChildAlongAxisWithScale(child, axis, pos + offsetInCell, scaleFactor);
    //                 }
    //                 pos += childSize * scaleFactor + spacing;
    //             }
    //         }
    //     }
}