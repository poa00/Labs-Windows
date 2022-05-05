// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !WINAPPSDK
using Windows.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml.Controls;
#endif

namespace CommunityToolkit.Labs.WinUI;

/// <summary>
/// The <see cref="ContentSizer"/> is a control which can be used to resize any element, usually its parent. If you are using a <see cref="Grid"/>, use <see cref="GridSplitter"/> instead.
/// </summary>
public partial class ContentSizer : SizerBase
{
}
