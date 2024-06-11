using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class PaletteLimitingShader : Sprite2D
{
	private ShaderMaterial _shaderMaterial;
	private const int MAX_NUM_PALETTE_COLORS = 1024;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shaderMaterial = Material as ShaderMaterial;
		ConnectFilesDroppedSignal();
	}

	private void ConnectFilesDroppedSignal()
	{
		var root = GetTree().Root;
		root.FilesDropped += OnFilesDropped;
	}

	private void OnFilesDropped(string[] files)
	{
		if (IsFileHexColor(files[0]))
		{
			string text = File.ReadAllText(files[0]);
			string[] lines = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
			AddColorsToShader(lines);
			var applyPaletteCheckbox = Owner.GetNode<CheckBox>("ApplyPaletteCheckBox");
			applyPaletteCheckbox.Visible = true;
			applyPaletteCheckbox.ButtonPressed = true;
		}
	}

	private bool IsFileHexColor(string filename)
	{
		if (Path.GetExtension(filename).Equals(".hex"))
		{
			return true;
		}
		return false;
	}

	private void AddColorsToShader(string[] lines)
	{
		int numColorsInHexFile = lines.Count() - 1;
		List<Color> colors = new List<Color>();

		if (lines.Count() > MAX_NUM_PALETTE_COLORS + 1) // +1 since the last new-line in the .hex file is counted as well.
		{
			GD.PrintErr($"Max number of palette colors is {MAX_NUM_PALETTE_COLORS}, The number of colors in the hex file is:{numColorsInHexFile}");
			return;
		}

		foreach (var line in lines)
		{
			// if the line is null or empty
			if (string.IsNullOrEmpty(line))
			{
				continue;   // Go to the next line
			}
			Color newColor = new Color(line);   // Get the color	
			colors.Add(newColor);
		}
		_shaderMaterial.Set("shader_parameter/colors", colors.ToArray());       // Set the colors in the shader		
		_shaderMaterial.Set("shader_parameter/num_colors", numColorsInHexFile); // Set the number of colors in the shader		
		this.Visible = true;                                                    // Make the palette indexer visible		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
