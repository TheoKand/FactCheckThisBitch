# OpenShot Video Editor is a program that creates, modifies, and edits video files.
#   Copyright (C) 2009  Jonathan Thomas
#
# This file is part of OpenShot Video Editor (http://launchpad.net/openshot/).
#
# OpenShot Video Editor is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# OpenShot Video Editor is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with OpenShot Video Editor.  If not, see <http://www.gnu.org/licenses/>.


# Import Blender's python API.  This only works when the script is being
# run from the context of Blender.  Blender contains it's own version of Python
# with this library pre-installed.
import bpy
import json


def load_font(font_path):
    """ Load a new TTF font into Blender, and return the font object """
    # get the original list of fonts (before we add a new one)
    original_fonts = bpy.data.fonts.keys()

    # load new font
    bpy.ops.font.open(filepath=font_path)

    # get the new list of fonts (after we added a new one)
    for font_name in bpy.data.fonts.keys():
        if font_name not in original_fonts:
            return bpy.data.fonts[font_name]

    # no new font was added
    return None

# Debug Info:
# ./blender -b test.blend -P demo.py
# -b = background mode
# -P = run a Python script within the context of the project file


# Init all of the variables needed by this script.  Because Blender executes
# this script, OpenShot will inject a dictionary of the required parameters
# before this script is executed.
params = {
    'title': 'Oh Yeah! OpenShot!',
    'extrude': 0.1,
    'bevel_depth': 0.02,
    'spacemode': 'CENTER',
    'text_size': 1.5,
    'width': 1.0,
    'fontname': 'Bfont',

    'color': [0.8, 0.8, 0.8],
    'alpha': 1.0,

    'output_path': '/tmp/',
    'fps': 24,
    'quality': 90,
    'file_format': 'PNG',
    'color_mode': 'RGBA',
    'horizon_color': [0.57, 0.57, 0.57],
    'resolution_x': 1920,
    'resolution_y': 1080,
    'resolution_percentage': 100,
    'start_frame': 1,
    'end_frame': 300,
    'animation': True,
}


#BEGIN INJECTING PARAMS
params_json = r"""{"file_name": "TitleFileName", "title1": "Title 1", "title2": "Title 2", "title3": "Title 3", "extrude": 0.1, "bevel_depth": 0.02, "fontname": "Bfont", "text_size": 1.0, "width": 1.0, "diffuse_color": [1.0, 1.0, 1.0, 1.0], "specular_color": [1.0, 1.0, 1.0], "specular_intensity": 0.5, "specular_color_bg": [1.0, 1.0, 1.0], "specular_intensity_bg": 0.5, "shadeless": "No", "diffuse_color_t1": [0.0, 0.5450980392156862, 0.9058823529411765, 1.0], "diffuse_color_t2": [0.9058823529411765, 0.0, 0.7529411764705882, 1.0], "diffuse_color_t3": [0.9529411764705882, 0.6784313725490196, 0.0, 1.0], "diffuse_color_t4": [0.0, 0.0, 0.0, 1.0], "start_frame": 1, "end_frame": 300, "length_multiplier": "1", "fps": 30, "resolution_x": 1280, "resolution_y": 720, "resolution_percentage": 50, "quality": 100, "file_format": "PNG", "color_mode": "RGBA", "alpha_mode": 1, "horizon_color": [0.57, 0.57, 0.57], "animation": true, "output_path": "C:\\Users\\theok\\Repos\\FactCheckThisBitch\\OpenShot\\RAIN-WeAreAllInThisTogether\\RAIN-WeAreAllInThisTogether-MoveRight_assets\\blender\\HKVYDCOZWQ\\TitleFileName"}"""
#END INJECTING PARAMS


# The remainder of this script will modify the current Blender .blend project
# file, and adjust the settings.  The .blend file is specified in the XML file
# that defines this template in OpenShot.
# ----------------------------------------------------------------------------

# Process parameters supplied as JSON serialization
try:
    injected_params = json.loads(params_json)
    params.update(injected_params)
except NameError:
    pass

# Get font object
font = None
if params["fontname"] != "Bfont":
    # Add font so it's available to Blender
    font = load_font(params["fontname"])
else:
    # Get default font
    font = bpy.data.fonts["Bfont"]

# TITLE 1 - Modify Text / Curve settings
text_object1 = bpy.data.curves["Title1"]
text_object1.body = params["title1"]

# TITLE 2 - Modify Text / Curve settings
text_object2 = bpy.data.curves["Title2"]
text_object2.body = params["title2"]

# TITLE 3 - Modify Text / Curve settings
text_object3 = bpy.data.curves["Title3"]
text_object3.body = params["title3"]

# Set common text properties
for ob in [text_object1, text_object2, text_object3]:
    ob.extrude = params["extrude"]
    ob.bevel_depth = params["bevel_depth"]
    ob.align_x = params["spacemode"]
    ob.size = params["text_size"]
    ob.space_character = params["width"]
    ob.font = font

# Change the title material settings (color, alpha, etc...)
for mat in [
        "Title1.Material",
        "Title2.Material",
        "Title3.Material",
        ]:
    ob = bpy.data.materials[mat]
    ob.diffuse_color = params["diffuse_color"]
    ob.specular_color = params["specular_color"]
    ob.specular_intensity = params["specular_intensity"]

# BACKGROUND - Change the material settings (color, alpha, etc...)
bg_mat = bpy.data.materials["Background.Material"]
bg_mat.specular_color = params["specular_color_bg"]
bg_mat.specular_intensity = params["specular_intensity_bg"]

# Shadeless Background
# TODO: Unsupported in Blender 2.8 (not sure of workaround yet)
# if params["shadeless"] == "Yes":
# material_object4.use_shadeless = True
# else:
# material_object4.use_shadeless = False


def update_keyframe(point, coord):
    point.co = coord
    point.handle_left.y = coord[1]
    point.handle_right.y = coord[1]


def update_curves(action, point, frame, color):
    for i in range(0, 3):
        coord = (frame, color[i])
        update_keyframe(
            action.fcurves[i].keyframe_points[point],
            coord)


# BACKGROUND COLORS (KEYFRAMES) ----------------------
action = bpy.data.actions["Background.MaterialAc"]

# TILE 1
update_curves(action, 0, 1.0, params["diffuse_color_t1"])
update_curves(action, 1, 70.0, params["diffuse_color_t1"])

# TILE 2
update_curves(action, 2, 120.0, params["diffuse_color_t2"])
update_curves(action, 3, 160.0, params["diffuse_color_t2"])

# TILE 3
update_curves(action, 4, 200.0, params["diffuse_color_t3"])
update_curves(action, 5, 240.0, params["diffuse_color_t3"])

# TILE 4
update_curves(action, 6, 300.0, params["diffuse_color_t4"])

# Set the render options.  It is important that these are set
# to the same values as the current OpenShot project.  These
# params are automatically set by OpenShot
render = bpy.context.scene.render
render.filepath = params["output_path"]
render.fps = params["fps"]
if "fps_base" in params:
    render.fps_base = params["fps_base"]
render.image_settings.file_format = params["file_format"]
render.image_settings.color_mode = params["color_mode"]
render.film_transparent = params["alpha_mode"]
bpy.data.worlds[0].color = params["horizon_color"]
render.resolution_x = params["resolution_x"]
render.resolution_y = params["resolution_y"]
render.resolution_percentage = params["resolution_percentage"]

# Animation Speed (use Blender's time remapping to slow or speed up animation)
length_multiplier = int(params["length_multiplier"])  # time remapping multiplier
new_length = int(params["end_frame"]) * length_multiplier  # new length (in frames)
render.frame_map_old = 1
render.frame_map_new = length_multiplier

# Set render length/position
bpy.context.scene.frame_start = params["start_frame"]
bpy.context.scene.frame_end = new_length
