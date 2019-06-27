
function AlienShipToy::create( %this )
{
    // Reset the toy.
    AlienShipToy.reset();
    AlienShipToy.GlobalPrevTime=getSimTime();
    AlienShipToy.HitCount=0;
    AlienShipToy.ScoreValue=0;
    AlienShipToy.HealthValue=100;
    AlienShipToy.FuelValue=100;
    AlienShipToy.MissileHit=getSimTime();
    %music = alxPlay("AlienShipToy:background");
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
    %this.createDebris(0);
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
    
    %object2 = new ImageFont();
    AlienShipToy.Score=%object2;
    %object2.Image = "ToyAssets:Font";
    %object2.Position = "35  30";
    %object2.FontSize = "3 3";
    %object2.FontPadding = 0;
    %object2.TextAlignment = "Center";
    %object2.Text = "Score 0";
    %object2.BodyType = "static";
    SandboxScene.add( %object2 ); 
    
    //Add health bar
    %healthText = new ImageFont();
    AlienShipToy.Health=%healthText;
    %healthText.Image = "ToyAssets:Font";
    %healthText.Position = "-37 30";
    %healthText.FontSize = "3 3";
    %healthText.FontPadding = 0;
    %healthText.TextAlignment = "Center";
    %healthText.Text = "100";
    %healthText.BodyType = "static";
    SandboxScene.add( %healthText );
    
    %healthBar=new Sprite();
    %healthBar.setImage("AlienShipToy:heart");
    %healthBar.setPosition(-45,30);
    %healthBar.setSize(5, 5);
    SandboxScene.add( %healthBar ); 
    
    //Add fuel bar
    %fuelText = new ImageFont();
    AlienShipToy.Fuel=%fuelText;
    %fuelText.Image = "ToyAssets:Font";
    %fuelText.Position = "-37 25";
    %fuelText.FontSize = "3 3";
    %fuelText.FontPadding = 0;
    %fuelText.TextAlignment = "Center";
    %fuelText.Text = "100";
    %fuelText.BodyType = "static";
    SandboxScene.add( %fuelText );
    
    %fuelBar=new Sprite();
    %fuelBar.setImage("AlienShipToy:fuel");
    %fuelBar.setPosition(-45,25);
    %fuelBar.setSize(4, 4);
    SandboxScene.add( %fuelBar );
    
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

function AlienShipToy::createDebris(%this,%val)
{
    
    if(%val==0)
    {
        %player=new Sprite();
        AlienShipToy.Player=%player;
        //%randomPosition = getRandom(-10, 10) SPC getRandom(2, 8);
        %player.setImage("AlienShipToy:ship");
        %player.setPosition(-40,0);
        %player.setSize(20, 15);
        %player.setDefaultFriction(1.0);
        %player.setDefaultDensity(0.1);
        %player.createPolygonBoxCollisionShape(15, 15);
        %player.setBullet( true );
        SandboxScene.add( %player ); 
    }else if(%val=1){
        $TestArray[0]="AlienShipToy:bomb";
        $TestArray[1]="AlienShipToy:tnt";
        $TestArray[2]="AlienShipToy:banana";
        $TestArray[3]="AlienShipToy:apple";
        $TestArray[4]="AlienShipToy:strawberry";
        $TestArray[5]="AlienShipToy:fuelTank";
        
        %randomArrayIndex1=getRandom(0, 5);
        %randomPosition = getRandom(50, 80) SPC getRandom(-40, 30);
        %obj = new Sprite();   
        AlienShipToy.Enemy1=%obj;
        AlienShipToy.Enemy1Type=%randomArrayIndex1;
        %obj.setImage($TestArray[%randomArrayIndex1]);
        %obj.setPosition(%randomPosition);
        %obj.setSize(8, 8);
        %obj.setDefaultFriction(1.0);
        %obj.setDefaultDensity(0.1);
        %obj.createPolygonBoxCollisionShape(4, 4);
        %obj.setBullet( true );
        %obj.setLinearVelocity(-20,0);
        SandboxScene.add(%obj);
        
        //Enemy 2
        %randomArrayIndex2=getRandom(0, 5);
        %randomPosition2 = getRandom(50, 80) SPC getRandom(-40, 30);
        %obj2 = new Sprite();   
        AlienShipToy.Enemy2=%obj2;
        AlienShipToy.Enemy2Type=%randomArrayIndex2;
        %obj2.setImage($TestArray[%randomArrayIndex2]);
        %obj2.setPosition(%randomPosition2);
        %obj2.setSize(8, 8);
        %obj2.setDefaultFriction(1.0);
        %obj2.setDefaultDensity(0.1);
        %obj2.createPolygonBoxCollisionShape(4, 4);
        %obj2.setBullet( true );
        %obj2.setLinearVelocity(-20,0);
        SandboxScene.add(%obj2);
        
        //Enemy 3
        %randomArrayIndex3=getRandom(0, 5);
        %randomPosition3 = getRandom(50, 80) SPC getRandom(-40, 30);
        %obj3 = new Sprite();   
        AlienShipToy.Enemy3=%obj3;
        AlienShipToy.Enemy3Type=%randomArrayIndex3;
        %obj3.setImage($TestArray[%randomArrayIndex3]);
        %obj3.setPosition(%randomPosition3 );
        %obj3.setSize(8, 8);
        %obj3.setDefaultFriction(1.0);
        %obj3.setDefaultDensity(0.1);
        %obj3.createPolygonBoxCollisionShape(4, 4);
        %obj3.setBullet( true );
        %obj3.setLinearVelocity(-20,0);
        SandboxScene.add(%obj3);
        
        
    }
}

//-----------------------------------------------------------------------------

function AlienShipToy::onTouchMoved(%this, %touchID, %worldPosition)
{
    // Set the scrollers speed to be the distance from the farground scrollers origin.
    // Also use the sign to control the direction of scrolling.
    %playerX = %worldPosition.x;
    %playerY = %worldPosition.y;
    
    // Set the scroller speeds.
    AlienShipToy.Player.Position.x = %playerX;
    AlienShipToy.Player.Position.y = %playerY;
    
    %playerx=AlienShipToy.Player.Position.x;
    %playery=AlienShipToy.Player.Position.y;
    
//    echo("++++Player pos++++");
//    echo(%playerx);
//    echo(%playery);
    
    if(%playerx>-14){
        AlienShipToy.Player.Position.x=-15;
    }
    if(%playerx<-38){
        AlienShipToy.Player.Position.x=-38;
    }
    
    %tempFuelValue=AlienShipToy.FuelValue;
    if(%tempFuelValue<0){
        AlienShipToy.FuelValue=0;
        AlienShipToy.Fuel.Text=0;
        //Add fuel bar
        %overText = new ImageFont();
        %overText.Image = "ToyAssets:Font";
        %overText.Position = "0 0";
        %overText.FontSize = "8 8";
        %overText.FontPadding = 0;
        %overText.TextAlignment = "Center";
        %overText.Text = "Out of Fuel";
        %overText.BodyType = "static";
        SandboxScene.add( %overText );
        
        AlienShipToy.onTouchMoved(null,null,null);
        AlienShipToy.onTouchDown(null,null,null);
        AlienShipToy.Player.Position.x=0;
        AlienShipToy.Player.Position.y=0;
    }
    %tempHealthValue=AlienShipToy.HealthValue;
    if(%tempHealthValue<0){
        AlienShipToy.HealthValue=0;
        AlienShipToy.Health.Text=0;
        //Add fuel bar
        %overText = new ImageFont();
        %overText.Image = "ToyAssets:Font";
        %overText.Position = "0 0";
        %overText.FontSize = "8 8";
        %overText.FontPadding = 0;
        %overText.TextAlignment = "Center";
        %overText.Text = "Alien Died";
        %overText.BodyType = "static";
        SandboxScene.add( %overText );
        
        for(%i=0;%i<10;%i++){
            %randomPosition4 = getRandom(-10, 20) SPC getRandom(-40, 30);
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%randomPosition4;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
        }
        
        AlienShipToy.onTouchMoved(null,null,null);
        AlienShipToy.onTouchDown(null,null,null);
        AlienShipToy.Player.Position.x=0;
        AlienShipToy.Player.Position.y=0;
    }
    
    %newTime=getSimTime();
    %prevTime=AlienShipToy.GlobalPrevTime;
    %timeGap=(%newTime-%prevTime)/1000;
    if(%timeGap>10){
        //echo("Time gap > 10");
        AlienShipToy.createDebris(1);
        AlienShipToy.GlobalPrevTime=getSimTime();
    }
    
    %playerPositionX=AlienShipToy.Player.Position.x;
    %playerPositionY=AlienShipToy.Player.Position.y;
    
    %missilePositionX=AlienShipToy.Missile.Position.x;
    %missilePositionY=AlienShipToy.Missile.Position.y;
    
    %enemy1PositionX=AlienShipToy.Enemy1.Position.x;
    %enemy1PositionY=AlienShipToy.Enemy1.Position.y;
    
    %enemy2PositionX=AlienShipToy.Enemy2.Position.x;
    %enemy2PositionY=AlienShipToy.Enemy2.Position.y;
    
    %enemy3PositionX=AlienShipToy.Enemy3.Position.x;
    %enemy3PositionY=AlienShipToy.Enemy3.Position.y;
    
    %distance1x=MAbs(%enemy1PositionX-%playerPositionX);
    %distance2x=MAbs(%enemy2PositionX-%playerPositionX);
    %distance3x=MAbs(%enemy3PositionX-%playerPositionX);
    %distance1y=MAbs(%enemy1PositionY-%playerPositionY);
    %distance2y=MAbs(%enemy2PositionY-%playerPositionY);
    %distance3y=MAbs(%enemy3PositionY-%playerPositionY);
    
    %distanceMissile1X=MAbs(%enemy1PositionX-%missilePositionX);
    %distanceMissile1Y=MAbs(%enemy1PositionY-%missilePositionY);
    
    %distanceMissile2X=MAbs(%enemy2PositionX-%missilePositionX);
    %distanceMissile2Y=MAbs(%enemy2PositionY-%missilePositionY);
    
    %distanceMissile3X=MAbs(%enemy3PositionX-%missilePositionX);
    %distanceMissile3Y=MAbs(%enemy3PositionY-%missilePositionY);
    
    //Enemy types
    %enemy1Type=AlienShipToy.Enemy1Type;
    %enemy2Type=AlienShipToy.Enemy2Type;
    %enemy3Type=AlienShipToy.Enemy3Type;
    
    if((%distance1x>0 && %distance1x<12)&&(%distance1y>0 && %distance1y<12)){
        echo("Collide with ship");
        //echo(%distance1x);
        if(%enemy1Type==0||%enemy1Type==1){
            AlienShipToy.HitCount++;
            AlienShipToy.HealthValue-=5;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            //Add explosion particles
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%playerPositionX @ %playerPositionY;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }else if(%enemy1Type==2||%enemy1Type==3||%enemy1Type==4){
           if(AlienShipToy.HealthValue!=100){
                AlienShipToy.HealthValue++;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }
        }else if(%enemy1Type==5){
            AlienShipToy.FuelValue+=8;
            %fuel=AlienShipToy.FuelValue;
            AlienShipToy.Fuel.Text=%fuel;
            %music = alxPlay("AlienShipToy:fuelUp");
        }
        AlienShipToy.Enemy1.delete();
        %distance=0;
    }else if((%distance2x>0 && %distance2x<12)&&(%distance2y>0 && %distance2y<12)){
        echo("Collide with ship");
        if(%enemy2Type==0||%enemy2Type==1){
            AlienShipToy.HitCount++;
            AlienShipToy.HealthValue-=5;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%playerPositionX @ %playerPositionY;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }else if(%enemy2Type==2||%enemy2Type==3||%enemy2Type==4){
            if(AlienShipToy.HealthValue!=100){
                AlienShipToy.HealthValue++;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }
        }else if(%enemy2Type==5){
            AlienShipToy.FuelValue+=8;
            %fuel=AlienShipToy.FuelValue;
            AlienShipToy.Fuel.Text=%fuel;
            %music = alxPlay("AlienShipToy:fuelUp");
        }
        AlienShipToy.Enemy2.delete();
        %distance=0;
    }else if((%distance3x>0 && %distance3x<12)&&(%distance3y>0 && %distance3y<12)){
        echo("Collide with ship");
        if(%enemy3Type==0||%enemy3Type==1){
            AlienShipToy.HitCount++;
            AlienShipToy.HealthValue-=5;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%playerPositionX @ %playerPositionY;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }else if(%enemy3Type==2||%enemy3Type==3||%enemy3Type==4){
            if(AlienShipToy.HealthValue!=100){
                AlienShipToy.HealthValue++;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }
        }else if(%enemy3Type==5){
            AlienShipToy.FuelValue+=8;
            %fuel=AlienShipToy.FuelValue;
            AlienShipToy.Fuel.Text=%fuel;
            %music = alxPlay("AlienShipToy:fuelUp");
        }
        AlienShipToy.Enemy3.delete();
        %distance=0;
    }
    
    if(%enemy1PositionX<-35){
        echo("Collide not with ship");
        AlienShipToy.Enemy1.delete();
        if(%enemy1Type==0||%enemy1Type==1){
            AlienShipToy.HealthValue-=3;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%playerPositionX @ %playerPositionY;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }
    }else if(%enemy3PositionX<-35){
        echo("Collide not with ship");
        AlienShipToy.Enemy3.delete();
        if(%enemy3Type==0||%enemy3Type==1){
            AlienShipToy.HealthValue-=3;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%playerPositionX @ %playerPositionY;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
        }
    }else if(%enemy2PositionX<-35){
        echo("Collide not with ship");
        AlienShipToy.Enemy2.delete();
        if(%enemy2Type==0||%enemy2Type==1){
            AlienShipToy.HealthValue-=3;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = Static;
            %particlePlayer.Position=%playerPositionX @ %playerPositionY;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
        }
    }
    
    if((%distanceMissile1X<14 && %distanceMissile1X>0)&&(%distanceMissile1Y<10 && %distanceMissile1Y>0)){
        echo("+++++++++++Enemy1");
        if(%enemy1Type==0||%enemy1Type==1){
            AlienShipToy.ScoreValue++;
            %score=AlienShipToy.ScoreValue;
            AlienShipToy.Score.Text="Score " @ %score;
        }
        %particlePlayer = new ParticlePlayer();
        %particlePlayer.BodyType = Static;
        %particlePlayer.Position=%missilePositionX @ %missilePositionY-5;    
        %particlePlayer.Size="10";
        %particlePlayer.SceneLayer = 31;
        %particlePlayer.ParticleInterpolation = true;
        %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
        %particlePlayer.SizeScale = 4;
        SandboxScene.add( %particlePlayer );
        %music = alxPlay("AlienShipToy:bombing");
        AlienShipToy.Enemy1.delete();
        AlienShipToy.Missile.delete();
        %distanceMissile1X=0;
        %distanceMissile1Y=0;
    }
    
    else if((%distanceMissile2X<14 && %distanceMissile2X>0)&&(%distanceMissile2Y<10 && %distanceMissile2Y>0)){
        echo("+++++++++++Enemy2");
        if(%enemy2Type==0||%enemy2Type==1){
            AlienShipToy.ScoreValue++;
            %score=AlienShipToy.ScoreValue;
            AlienShipToy.Score.Text="Score " @ %score;
        }
        %particlePlayer = new ParticlePlayer();
        %particlePlayer.BodyType = Static;
        %particlePlayer.Position=%missilePositionX @ %missilePositionY-5;    
        %particlePlayer.Size="10";
        %particlePlayer.SceneLayer = 31;
        %particlePlayer.ParticleInterpolation = true;
        %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
        %particlePlayer.SizeScale = 4;
        SandboxScene.add( %particlePlayer );
        %music = alxPlay("AlienShipToy:bombing");
        
        AlienShipToy.Enemy2.delete();
        AlienShipToy.Missile.delete();
        %distanceMissile2X=0;
        %distanceMissile2Y=0;
    }
    else if((%distanceMissile3X<14 && %distanceMissile3X>0)&&(%distanceMissile3Y<10 && %distanceMissile3Y>0)){
        echo("+++++++++++Enemy3");
        if(%enemy3Type==0||%enemy3Type==1){
            AlienShipToy.ScoreValue++;
            %score=AlienShipToy.ScoreValue;
            AlienShipToy.Score.Text="Score " @ %score;
            
        }
        %particlePlayer = new ParticlePlayer();
        %particlePlayer.BodyType = Static;
        %particlePlayer.Position=%missilePositionX @ %missilePositionY-5;    
        %particlePlayer.Size="10";
        %particlePlayer.SceneLayer = 31;
        %particlePlayer.ParticleInterpolation = true;
        %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
        %particlePlayer.SizeScale = 4;
        SandboxScene.add( %particlePlayer );
        %music = alxPlay("AlienShipToy:bombing");
        
        AlienShipToy.Enemy3.delete();
        AlienShipToy.Missile.delete();
        %distanceMissile3X=0;
        %distanceMissile3Y=0;
    }
    
    AlienShipToy.Player.SetAngularVelocity(0);
    AlienShipToy.Player.SetLinearVelocity(0,0);
    AlienShipToy.FuelValue-=0.01;
    %tempFuelValue=mCeil(AlienShipToy.FuelValue);
    AlienShipToy.Fuel.Text=%tempFuelValue;
}
function AlienShipToy::onTouchDown(%this, %touchID, %worldPosition)
{
    %missileHitTime=AlienShipToy.MissileHit;
    %newTime=getSimTime();
    %possibleTime= (%newTime-%missileHitTime)/1000;
    if(%possibleTime>10){
        //echo(%possibleTime);
        %missilePositionX=%worldPosition.x;
        %missilePositionY=%worldPosition.y;
        %missile=new Sprite();
        AlienShipToy.Missile=%missile;
        //%randomPosition = getRandom(-10, 10) SPC getRandom(2, 8);
        %missile.setImage("AlienShipToy:missile");
        %missile.setPosition(%missilePositionX,%missilePositionY-15);
        %missile.setSize(20, 7);
        %missile.setDefaultFriction(1.0);
        %missile.setDefaultDensity(0.1);
        %missile.createPolygonBoxCollisionShape(20, 10);
        %missile.setLinearVelocity(120,0);
        AlienShipToy.Player.SetAngularVelocity(0);
        SandboxScene.add( %missile );
        %music = alxPlay("AlienShipToy:missileSound");
        AlienShipToy.MissileHit=getSimTime();
    }
   
}

function AlienShipToy::onMiddleMouseDown(%this, %touchID, %worldPosition)
{
    AlienShipToy.create();
}


