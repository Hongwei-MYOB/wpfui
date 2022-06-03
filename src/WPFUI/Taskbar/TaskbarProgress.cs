﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using WPFUI.Appearance;
using WPFUI.Interop;

namespace WPFUI.Taskbar;

/// <summary>
/// Allows to change the status of the displayed notification in the application icon on the TaskBar.
/// </summary>
public static class TaskbarProgress
{
    /// <summary>
    /// Gets a value indicating whether the current operating system supports taskbar manipulation.
    /// </summary>
    /// <returns></returns>
    private static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows7OrNewer;
    }

    /// <summary>
    /// Allows to change the status of the progress bar in the taskbar.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <param name="progressState">State of the progress indicator.</param>
    public static bool SetState(Window window, ProgressState progressState)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return SetState(new WindowInteropHelper(window).Handle, progressState);

        window.Loaded += (_, _) =>
        {
            SetState(new WindowInteropHelper(window).Handle, progressState);
        };

        return true;
    }

    /// <summary>
    /// Allows to change the fill of the taskbar.
    /// </summary>
    /// <param name="window">Window to manipulate.</param>
    /// <param name="progressState">Progress sate to set.</param>
    /// <param name="current">Current value to display</param>
    /// <param name="total">Total number for division.</param>
    public static bool SetValue(Window window, ProgressState progressState, int current, int total)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return SetValue(new WindowInteropHelper(window).Handle, progressState, current, total);

        window.Loaded += (_, _) =>
        {
            SetValue(new WindowInteropHelper(window).Handle, progressState, current, total);
        };

        return false;
    }

    /// <summary>
    /// Allows to change the status of the progress bar in the taskbar.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="progressState">State of the progress indicator.</param>
    public static bool SetState(IntPtr hWnd, ProgressState progressState)
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available.");

        var nativeProgressState = progressState switch
        {
            ProgressState.Indeterminate => ShObjIdl.TBPFLAG.TBPF_INDETERMINATE,
            ProgressState.Error => ShObjIdl.TBPFLAG.TBPF_ERROR,
            ProgressState.Paused => ShObjIdl.TBPFLAG.TBPF_PAUSED,
            ProgressState.Normal => ShObjIdl.TBPFLAG.TBPF_NORMAL,
            _ => WPFUI.Interop.ShObjIdl.TBPFLAG.TBPF_NOPROGRESS
        };

        return UnsafeNativeMethods.SetTaskbarState(hWnd, nativeProgressState);
    }

    /// <summary>
    /// Allows to change the fill of the taskbar.
    /// </summary>
    /// <param name="hWnd">Window handle.</param>
    /// <param name="progressState">Progress sate to set.</param>
    /// <param name="current">Current value to display</param>
    /// <param name="total">Total number for division.</param>
    public static bool SetValue(IntPtr hWnd, ProgressState progressState, int current, int total)
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available.");

        var nativeProgressState = progressState switch
        {
            ProgressState.Indeterminate => ShObjIdl.TBPFLAG.TBPF_INDETERMINATE,
            ProgressState.Error => ShObjIdl.TBPFLAG.TBPF_ERROR,
            ProgressState.Paused => ShObjIdl.TBPFLAG.TBPF_PAUSED,
            ProgressState.Normal => ShObjIdl.TBPFLAG.TBPF_NORMAL,
            _ => WPFUI.Interop.ShObjIdl.TBPFLAG.TBPF_NOPROGRESS
        };

        return UnsafeNativeMethods.SetTaskbarValue(hWnd, nativeProgressState, current, total);
    }
}