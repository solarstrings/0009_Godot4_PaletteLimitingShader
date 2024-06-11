using Godot;
using System;

public partial class Main : Node2D
{
	private PaletteLimitingShader _paletteLimitingShader;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_paletteLimitingShader = GetNode<PaletteLimitingShader>("PaletteLimitingShader");
	}
	private void OnApplyPaletteCheckBoxToggled(bool toggled)
	{
		if (toggled)
			_paletteLimitingShader.Visible = true;
		else
			_paletteLimitingShader.Visible = false;

	}

}
