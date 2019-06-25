
function AlienShipToy::create( %this )
{
    // Reset the toy.
    AlienShipToy.reset();
}

//-----------------------------------------------------------------------------

function AlienShipToy::destroy( %this )
{
}

//-----------------------------------------------------------------------------

function AlienShipToy::reset( %this )
{
    // Clear the scene.
    SandboxScene.clear();
    
    // Create some scrollers.
    %this.createBackground();   
    %this.createFarScroller();
    %this.createNearScroller();
    %this.createDebris();
}

//-----------------------------------------------------------------------------

function AlienShipToy::createBackground( %this )
{    
    // Create the sprite.
    %object = new Sprite();
       
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 0";

    // Set the size.        
    %object.Size = "100 75";
    
    // Set to the furthest background layer.
    %object.SceneLayer = 31;
    
    // Set an image.
    %object.Image = "ToyAssets:jungleSky";
            
    // Add the sprite to the scene.
    SandboxScene.add( %object );    
}

//-----------------------------------------------------------------------------

function AlienShipToy::createFarScroller( %this )
{    
    // Create the scroller.
    %object = new Scroller();
    
    // Note this scroller for the touch controls.
    AlienShipToy.FarScroller = %object;
    
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 -10";

    // Set the size.        
    %object.Size = "100 75";

    // Set to the furthest background layer.
    %object.SceneLayer = 31;
    
    // Set the scroller to use a static image.
    %object.Image = "ToyAssets:TreeBackground2";
    
    // We don't really need to do this as the frame is set to zero by default.
    %object.Frame = 0;

    // Set the scroller moving in the X axis.
    %object.ScrollX = 10;
    
    // Set the scroller to only show half of the static image in the X axis.
    %object.RepeatX = 0.5;
        
    // Add the sprite to the scene.
    SandboxScene.add( %object );    
}

//-----------------------------------------------------------------------------

function AlienShipToy::createNearScroller( %this )
{    
    // Create the scroller.
    %object = new Scroller();

    // Note this scroller for the touch controls.
    AlienShipToy.NearScroller = %object;    
    
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 -10";

    // Set the size.        
    %object.Size = "100 75";
    
    // Set to the furthest background layer.
    %object.SceneLayer = 31;
    
    // Set the scroller to use a static image.
    %object.Image = "ToyAssets:TreeBackground1";
    
    // We don't really need to do this as the frame is set to zero by default.
    %object.Frame = 0;
    
    // Set the scroller moving in the X axis.
    %object.ScrollX = 20;

    // Set the scroller to only show half of the static image in the X axis.
    %object.RepeatX = 0.5;
    
    
    // Add the sprite to the scene.
    SandboxScene.add( %object );    
}

function AlienShipToy::createDebris(%this)
{
    %player=new Sprite();
    AlienShipToy.Player=%player;
    %randomPosition = getRandom(-10, 10) SPC getRandom(2, 8);
    %player.setImage("AlienShipToy:ship");
    %player.setPosition(%randomPosition);
    %player.setSize(10, 10);
    %player.setDefaultFriction(1.0);
    %player.setDefaultDensity(0.1);
    %player.createPolygonBoxCollisionShape(1.4, 1.4);
    %player.setBullet( true );
    SandboxScene.add( %player );
}



//-----------------------------------------------------------------------------

function AlienShipToy::onTouchDragged(%this, %touchID, %worldPosition)
{
    // Set the scrollers speed to be the distance from the farground scrollers origin.
    // Also use the sign to control the direction of scrolling.
    %scrollerX = %worldPosition.x;
    %scrollerY = %worldPosition.y;

    // Set the scroller speeds.
    AlienShipToy.Player.Position.x = %scrollerX;
    AlienShipToy.Player.Position.y = %scrollerY;
}
