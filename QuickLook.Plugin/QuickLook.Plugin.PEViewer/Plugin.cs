﻿// Copyright © 2017-2025 QL-Win Contributors
//
// This file is part of QuickLook program.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using QuickLook.Common.Plugin;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace QuickLook.Plugin.PEViewer;

public class Plugin : IViewer
{
    private static readonly string[] _extensions =
    [
        ".exe", ".sys", ".scr", ".ocx", ".cpl",  ".bpl",
        ".dll", ".ax", ".drv", ".vxd",
        ".mui", ".mun",
        ".tlb",
        ".efi", ".mz",
    ];

    private PEInfoPanel _ip;
    private string _path;

    public int Priority => 0;

    public void Init()
    {
    }

    public bool CanHandle(string path)
    {
        return !Directory.Exists(path) && _extensions.Any(path.ToLower().EndsWith);
    }

    public void Prepare(string path, ContextObject context)
    {
        context.PreferredSize = new Size { Width = 520, Height = 192 };
        context.Title = string.Empty;
        context.TitlebarOverlap = false;
        context.TitlebarBlurVisibility = false;
        context.TitlebarColourVisibility = false;
        context.FullWindowDragging = true;
    }

    public void View(string path, ContextObject context)
    {
        _path = path;
        _ip = new PEInfoPanel();

        _ip.DisplayInfo(_path);
        _ip.Tag = context;

        context.ViewerContent = _ip;
        context.IsBusy = false;
    }

    public void Cleanup()
    {
        GC.SuppressFinalize(this);

        _ip = null;
    }
}
