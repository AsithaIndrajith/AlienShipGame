
function AlienShipToy::create( %this )
{
    // Reset the toy.
    AlienShipToy.reset();
    AlienShipToy.GlobalPrevTime=getSimTime();
    AlienShipToy.GlobalTime=getSimTime();
    AlienShipToy.HitCount=0;
    AlienShipToy.ScoreValue=0;
    AlienShipToy.HealthValue=100;
    AlienShipToy.FuelValue=100;
    AlienShipToy.MissileHit=getSimTime();
    AlienShipToy.IsExplode=false;
    AlienShipToy.SetImage1=false;
    AlienShipToy.SetImage2=false;
    AlienShipToy.SetImage3=false;
    AlienShipToy.SetImage4=false;
    AlienShipToy.MissileCount=30;
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
    %object.Position = "0 0";        
    %object.Size = "100 75";
    %object.SceneLayer = 31;
    %object.Image = "ToyAssets:jungleSky";
            
    // Add the sprite to the scene.
    SandboxScene.add( %object ); 
    
    %object2 = new ImageFont();
    AlienShipToy.Score=%object2;
    %object2.Image = "ToyAssets:fancyFont";
    %object2.Position = "35 30";
    %object2.FontSize = "3 3";
    %object2.FontPadding = 0;
    %object2.TextAlignment = "Center";
    %object2.Text = "Score 0";
    %object2.BodyType = "static";
    SandboxScene.add( %object2 ); 
    
    %missileCountText = new ImageFont();
    AlienShipToy.MissileCountText=%missileCountText;
    %missileCountText.Image = "ToyAssets:fancyFont";
    %missileCountText.Position = "-37 20";
    %missileCountText.FontSize = "3 3";
    %missileCountText.FontPadding = 0;
    %missileCountText.TextAlignment = "Center";
    %missileCountText.Text = "30";
    %missileCountText.BodyType = "static";
    SandboxScene.add( %missileCountText );
    
    %missileCountBar=new Sprite();
    %missileCountBar.setImage("AlienShipToy:missile");
    %missileCountBar.setPosition(-45,20);
    %missileCountBar.setSize(5, 4);
    SandboxScene.add( %missileCountBar );
    
    %levelText = new ImageFont();
    AlienShipToy.LevelText=%levelText;
    %levelText.Image = "ToyAssets:fancyFont";
    %levelText.Position = "0  30";
    %levelText.FontSize = "3 3";
    %levelText.FontPadding = 0;
    %levelText.TextAlignment = "Center";
    %levelText.Text = "Level 1";
    %levelText.BodyType = "static";
    SandboxScene.add( %levelText );
    
    //Add health bar
    %healthText = new ImageFont();
    AlienShipToy.Health=%healthText;
    %healthText.Image = "ToyAssets:fancyFont";
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
    %fuelText.Image = "ToyAssets:fancyFont";
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
    AlienShipToy.FarScroller = %object;
    %object.Position = "0 -10";     
    %object.Size = "100 75";
    %object.SceneLayer = 31;
    %object.Image = "ToyAssets:TreeBackground2";
    %object.Frame = 0;
    %object.ScrollX = 10;
    %object.RepeatX = 0.5;
    SandboxScene.add( %object );    
}

//-----------------------------------------------------------------------------

function AlienShipToy::createNearScroller( %this )
{    
    %object = new Scroller();
    AlienShipToy.NearScroller = %object;
    %object.Position = "0 -10";    
    %object.Size = "100 75";
    %object.SceneLayer = 31;
    %object.Image = "ToyAssets:TreeBackground1";
    %object.Frame = 0;
    %object.ScrollX = 20;
    %object.RepeatX = 0.5;
    SandboxScene.add( %object ); 
    
    //Add fuel bar
    %bigLevelText = new ImageFont();
    AlienShipToy.BigLevelText=%bigLevelText;
    %bigLevelText.Image = "ToyAssets:fancyFont";
    %bigLevelText.Position = "0 0";
    %bigLevelText.FontSize = "4 4";
    %bigLevelText.FontPadding = 0;
    %bigLevelText.TextAlignment = "Center";
    %bigLevelText.Text = "Level 1";
    %bigLevelText.BodyType = "static";
    SandboxScene.add( %bigLevelText );
    
}

function AlienShipToy::createDebris(%this,%val)
{
    
    if(%val==0)
    {
        %player=new Sprite();
        AlienShipToy.Player=%player;
        //%randomPosition = getRandom(-10, 10) SPC getRandom(2, 8);
        %player.setImage("AlienShipToy:spaceship2");
        %player.setPosition(-40,0);
        %player.setSize(20, 15);
        %player.setDefaultFriction(1.0);
        %player.setDefaultDensity(0.1);
        %player.createPolygonBoxCollisionShape(15, 15);
        %player.setBullet( true );
        SandboxScene.add( %player ); 
        
        
    }else if(%val=1){
        $TestArray[0,0]="AlienShipToy:bomb";
        $TestArray[0,1]="AlienShipToy:tnt";
        $TestArray[0,2]="AlienShipToy:banana";
        $TestArray[0,3]="AlienShipToy:apple";
        $TestArray[0,4]="AlienShipToy:strawberry";
        $TestArray[0,5]="AlienShipToy:fuelTank";
        
        $TestArray[1,0]="AlienShipToy:bomb";
        $TestArray[1,1]="AlienShipToy:tnt";
        $TestArray[1,2]="AlienShipToy:banana";
        $TestArray[1,3]="AlienShipToy:apple";
        $TestArray[1,4]="AlienShipToy:strawberry";
        $TestArray[1,5]="AlienShipToy:fuelTank";
        
        $TestArray[2,0]="AlienShipToy:bomb";
        $TestArray[2,1]="AlienShipToy:tnt";
        $TestArray[2,2]="AlienShipToy:banana";
        $TestArray[2,3]="AlienShipToy:apple";
        $TestArray[2,4]="AlienShipToy:strawberry";
        $TestArray[2,5]="AlienShipToy:tnt";
        
        $TestArray[3,0]="AlienShipToy:bomb";
        $TestArray[3,1]="AlienShipToy:tnt";
        $TestArray[3,2]="AlienShipToy:banana";
        $TestArray[3,3]="AlienShipToy:apple";
        $TestArray[3,4]="AlienShipToy:strawberry";
        $TestArray[3,5]="AlienShipToy:bomb";
        
        $TestArray[4,0]="AlienShipToy:bomb";
        $TestArray[4,1]="AlienShipToy:tnt";
        $TestArray[4,2]="AlienShipToy:banana";
        $TestArray[4,3]="AlienShipToy:apple";
        $TestArray[4,4]="AlienShipToy:strawberry";
        $TestArray[4,5]="AlienShipToy:tnt";
        
        $TestArray[5,0]="AlienShipToy:bomb";
        $TestArray[5,1]="AlienShipToy:tnt";
        $TestArray[5,2]="AlienShipToy:bomb";
        $TestArray[5,3]="AlienShipToy:tnt";
        $TestArray[5,4]="AlienShipToy:fuelTank";
        $TestArray[5,5]="AlienShipToy:fuelTank";
        
        %randomArrayIndex1=getRandom(0, 5);
        %randomArrayIndex11=getRandom(0, 5);
        %randomPosition = getRandom(50, 80) SPC getRandom(-40, 30);
        %obj = new Sprite();   
        AlienShipToy.Enemy1=%obj;
        AlienShipToy.Enemy1Type=%randomArrayIndex1;
        %obj.setImage($TestArray[%randomArrayIndex1,%randomArrayIndex11]);
        %obj.setPosition(%randomPosition);
        %obj.setSize(8, 8);
        %obj.setDefaultFriction(1.0);
        %obj.setDefaultDensity(0.1);
        %obj.createPolygonBoxCollisionShape(4, 4);
        %obj.setBullet( true );
        
        SandboxScene.add(%obj);
        
        //Enemy 2
        %randomArrayIndex2=getRandom(0, 5);
        %randomArrayIndex22=getRandom(0, 5);
        %randomPosition2 = getRandom(50, 80) SPC getRandom(-40, 30);
        %obj2 = new Sprite();   
        AlienShipToy.Enemy2=%obj2;
        AlienShipToy.Enemy2Type=%randomArrayIndex2;
        %obj2.setImage($TestArray[%randomArrayIndex2,%randomArrayIndex22]);
        %obj2.setPosition(%randomPosition2);
        %obj2.setSize(8, 8);
        %obj2.setDefaultFriction(1.0);
        %obj2.setDefaultDensity(0.1);
        %obj2.createPolygonBoxCollisionShape(4, 4);
        %obj2.setBullet( true );
        
        SandboxScene.add(%obj2);
        
        //Enemy 3
        %randomArrayIndex3=getRandom(0, 5);
        %randomArrayIndex33=getRandom(0, 5);
        %randomPosition3 = getRandom(50, 80) SPC getRandom(-40, 30);
        %obj3 = new Sprite();   
        AlienShipToy.Enemy3=%obj3;
        AlienShipToy.Enemy3Type=%randomArrayIndex3;
        %obj3.setImage($TestArray[%randomArrayIndex3,%randomArrayIndex3]);
        %obj3.setPosition(%randomPosition3 );
        %obj3.setSize(8, 8);
        %obj3.setDefaultFriction(1.0);
        %obj3.setDefaultDensity(0.1);
        %obj3.createPolygonBoxCollisionShape(4, 4);
        %obj3.setBullet( true );
        
        SandboxScene.add(%obj3);
        
        %newTime=getSimTime();
        %prevTime=AlienShipToy.GlobalTime;
        %difference=(%newTime-prevTime)/1000;
        
        if(%difference<180){
            %obj.setLinearVelocity(-15,0);
            %obj2.setLinearVelocity(-15,0);
            %obj3.setLinearVelocity(-15,0);
        }else if(%difference>180 && %difference<420){
            AlienShipToy.LevelText.Text="Level 2";
            %obj.setLinearVelocity(-25,0);
            %obj2.setLinearVelocity(-25,0);
            %obj3.setLinearVelocity(-25,0);
        }else if(%difference>420 && %difference<720){
            AlienShipToy.LevelText.Text="Level 3";
            %obj.setLinearVelocity(-35,0);
            %obj2.setLinearVelocity(-35,0);
            %obj3.setLinearVelocity(-35,0);
        }else if(%difference>720 && %difference<1020){
            AlienShipToy.LevelText.Text="Level 4";
            %obj.setLinearVelocity(-45,0);
            %obj2.setLinearVelocity(-45,0);
            %obj3.setLinearVelocity(-45,0);
        }else if(%difference>1020 && %difference<1340){
            AlienShipToy.LevelText.Text="Level 5";
            %obj.setLinearVelocity(-55,0);
            %obj2.setLinearVelocity(-55,0);
            %obj3.setLinearVelocity(-55,0);
        }else if(%difference>1340 && %difference<1660){
            AlienShipToy.LevelText.Text="Level 6";
            %obj.setLinearVelocity(-65,0);
            %obj2.setLinearVelocity(-65,0);
            %obj3.setLinearVelocity(-65,0);
        }
    }
}

//-----------------------------------------------------------------------------

function AlienShipToy::onTouchMoved(%this, %touchID, %worldPosition)
{
    //Indicating the level upgrading
    %prevTime=AlienShipToy.GlobalTime;
    %newTime=getSimTime();
    %timeDifference=(%newTime-%prevTime)/1000;
    if(%timeDifference<6){
        AlienShipToy.BigLevelText.Text="Level 1";
    }else if(%timeDifference>6&&%timeDifference<180){
        AlienShipToy.BigLevelText.Text="Keep moving...";
        AlienShipToy.BigLevelText.Position="0 25";
        AlienShipToy.BigLevelText.FontSize="0 0";
    }else if(%timeDifference>180&&%timeDifference<186){
        AlienShipToy.BigLevelText.Text="Level 2";
        AlienShipToy.BigLevelText.Position="0 0";
        AlienShipToy.BigLevelText.FontSize="4 4";
    }else if(%timeDifference>186&&%timeDifference<420){
        AlienShipToy.BigLevelText.Text="Keep moving...";
        AlienShipToy.BigLevelText.Position="0 25";
        AlienShipToy.BigLevelText.FontSize="0 0";
    }else if(%timeDifference>420&&%timeDifference<426){
        AlienShipToy.BigLevelText.Text="Level 3";
        AlienShipToy.BigLevelText.Position="0 0";
        AlienShipToy.BigLevelText.FontSize="4 4";
    }
    
    //Changing the spaceship status
    %healthStatus=AlienShipToy.HealthValue;
    %setImage1 = AlienShipToy.SetImage1;
    %setImage1 = AlienShipToy.SetImage2;
    %setImage1 = AlienShipToy.SetImage3;
    %setImage1 = AlienShipToy.SetImage4;
    if(%healthStatus>=50 && %healthStatus<75 && !%setImage1){
        AlienShipToy.Player.setImage("AlienShipToy:spaceship2SmallCrack");
        AlienShipToy.SetImage1=true;
    }else if(%healthStatus>=25 && %healthStatus<50 && !%setImage2){
        AlienShipToy.Player.setImage("AlienShipToy:spaceship2MediumCrack");
        AlienShipToy.SetImage2=true;
    }else if(%healthStatus>=15 && %healthStatus<25 && !%setImage3){
        AlienShipToy.Player.setImage("AlienShipToy:spaceship2BigCrack");
        AlienShipToy.SetImage3=true;
    }else if(%healthStatus<15 && !%setImage4){
        AlienShipToy.Player.setImage("AlienShipToy:spaceship2HugeCrack");
        AlienShipToy.SetImage4=true;
        %music = alxPlay("AlienShipToy:alienSiren");
        AlienShipToy.AlienSiren=%music;
    }
    
    //Add rocket burning animation
    %bonfirePosition=AlienShipToy.Player.Position.x-12 SPC AlienShipToy.Player.Position.y-2;
    %bonfireatship = new ParticlePlayer();
    %bonfireatship.BodyType = "static";
    %bonfireatship.Position=%bonfirePosition;    
    %bonfireatship.Size="10";
    %bonfireatship.SceneLayer = 31;
    %bonfireatship.ParticleInterpolation = true;
    %bonfireatship.Particle = "AlienShipToy:rocketThruster";
    %bonfireatship.SizeScale = 4;
    SandboxScene.add( %bonfireatship );
    
    //Restrict player movement area
    %playerX = %worldPosition.x;
    %playerY = %worldPosition.y;
    
    AlienShipToy.Player.Position.x = %playerX;
    AlienShipToy.Player.Position.y = %playerY;
    
    %playerx=AlienShipToy.Player.Position.x;
    %playery=AlienShipToy.Player.Position.y;
    
    if(%playerx>-14){
        AlienShipToy.Player.Position.x=-15;
    }
    if(%playerx<-38){
        AlienShipToy.Player.Position.x=-37;
    }
    if(%playery>30){
        AlienShipToy.Player.Position.y=29;
    }
    if(%playery<-30){
        AlienShipToy.Player.Position.y=-29;
    }
    
    //End the game if fuel or health insufficient
    %tempFuelValue=AlienShipToy.FuelValue;
    if(%tempFuelValue<0){
        AlienShipToy.FuelValue=0;
        AlienShipToy.Fuel.Text=0;
        //Add fuel bar
        %overText = new ImageFont();
        %overText.Image = "ToyAssets:fancyFont";
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
        %overText.Image = "ToyAssets:fancyFont";
        %overText.Position = "0 0";
        %overText.FontSize = "6 6";
        %overText.FontPadding = 0;
        %overText.TextAlignment = "Center";
        %overText.Text = "Alien Died";
        %overText.BodyType = "static";
        SandboxScene.add( %overText );
        
        for(%i=0;%i<10;%i++){
            %randomPosition4 = getRandom(-10, 20) SPC getRandom(-40, 30);
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
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
    if(%timeGap>8){
        AlienShipToy.createDebris(1);
        AlienShipToy.GlobalPrevTime=getSimTime();
    }
    
    //get object positions to calculate collisions
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
        echo("1 Collide with ship");
        
        //echo(%distance1x);
        if(%enemy1Type==0||%enemy1Type==1){
            AlienShipToy.HitCount++;
            AlienShipToy.HealthValue-=10;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            //Add explosion particles
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%worldPosition.x SPC %worldPosition.y;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }else if(%enemy1Type==2||%enemy1Type==3||%enemy1Type==4){
           if(AlienShipToy.HealthValue+1<100){
                AlienShipToy.HealthValue++;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }else{
                AlienShipToy.HealthValue+=100-AlienShipToy.HealthValue;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }
        }else if(%enemy1Type==5){
            if( AlienShipToy.FuelValue+20<100){
                AlienShipToy.FuelValue+=20;
                %fuel=AlienShipToy.FuelValue;
                AlienShipToy.Fuel.Text=%fuel;
                %music = alxPlay("AlienShipToy:fuelUp");
            }else{
                AlienShipToy.FuelValue+=100-AlienShipToy.FuelValue;
                %fuel=AlienShipToy.FuelValue;
                AlienShipToy.Fuel.Text=%fuel;
                %music = alxPlay("AlienShipToy:fuelUp");
            }
        }
        AlienShipToy.Enemy1.delete();
        %distance=0;
    }else if((%distance2x>0 && %distance2x<12)&&(%distance2y>0 && %distance2y<12)){
        echo("2 Collide with ship");
        
        if(%enemy2Type==0||%enemy2Type==1){
            AlienShipToy.HitCount++;
            AlienShipToy.HealthValue-=10;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%worldPosition.x SPC %worldPosition.y;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }else if(%enemy2Type==2||%enemy2Type==3||%enemy2Type==4){
            if(AlienShipToy.HealthValue+1<100){
                AlienShipToy.HealthValue++;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }else{
                AlienShipToy.HealthValue+=100-AlienShipToy.HealthValue;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }
        }else if(%enemy2Type==5){
            if( AlienShipToy.FuelValue+20<100){
                AlienShipToy.FuelValue+=20;
                %fuel=AlienShipToy.FuelValue;
                AlienShipToy.Fuel.Text=%fuel;
                %music = alxPlay("AlienShipToy:fuelUp");
            }else{
                AlienShipToy.FuelValue+=100-AlienShipToy.FuelValue;
                %fuel=AlienShipToy.FuelValue;
                AlienShipToy.Fuel.Text=%fuel;
                %music = alxPlay("AlienShipToy:fuelUp");
            }
        }
        AlienShipToy.Enemy2.delete();
        %distance=0;
    }else if((%distance3x>0 && %distance3x<12)&&(%distance3y>0 && %distance3y<12)){
        echo("3 Collide with ship");
        
        if(%enemy3Type==0||%enemy3Type==1){
            AlienShipToy.HitCount++;
            AlienShipToy.HealthValue-=10;
            %health=AlienShipToy.HealthValue;
            AlienShipToy.Health.Text=%health;
            
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%worldPosition.x SPC %worldPosition.y;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
            
        }else if(%enemy3Type==2||%enemy3Type==3||%enemy3Type==4){
            if(AlienShipToy.HealthValue+1<100){
                AlienShipToy.HealthValue++;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }else{
                AlienShipToy.HealthValue+=100-AlienShipToy.HealthValue;
                %health=AlienShipToy.HealthValue;
                AlienShipToy.Health.Text=%health;
                %music = alxPlay("AlienShipToy:eatFruit");
            }
        }else if(%enemy3Type==5){
            if( AlienShipToy.FuelValue+20<100){
                AlienShipToy.FuelValue+=20;
                %fuel=AlienShipToy.FuelValue;
                AlienShipToy.Fuel.Text=%fuel;
                %music = alxPlay("AlienShipToy:fuelUp");
            }else{
                AlienShipToy.FuelValue+=100-AlienShipToy.FuelValue;
                %fuel=AlienShipToy.FuelValue;
                AlienShipToy.Fuel.Text=%fuel;
                %music = alxPlay("AlienShipToy:fuelUp");
            }
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
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%worldPosition.x SPC %worldPosition.y;    
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
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%worldPosition.x SPC %worldPosition.y;    
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
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%worldPosition.x SPC %worldPosition.y;    
            %particlePlayer.Size="10";
            %particlePlayer.SceneLayer = 31;
            %particlePlayer.ParticleInterpolation = true;
            %particlePlayer.Particle = "ToyAssets:ImpactExplosion";
            %particlePlayer.SizeScale = 4;
            SandboxScene.add( %particlePlayer );
            %music = alxPlay("AlienShipToy:bombing");
        }
    }
    %explode=AlienShipToy.IsExplode;
    
    if((%distanceMissile1X<14 && %distanceMissile1X>0)&&(%distanceMissile1Y<10 && %distanceMissile1Y>0)&& %explode){
        echo("1 Collide with missile");
        if(%enemy1PositionX!=null){
            %tempMissilePositionX=AlienShipToy.Missile.Position.x;
            %tempMissilePositionY=AlienShipToy.Missile.Position.y;

            if(%enemy1Type==0||%enemy1Type==1){
                AlienShipToy.ScoreValue+=10;
                %score=AlienShipToy.ScoreValue;
                AlienShipToy.Score.Text="Score " @ %score;
            }
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%tempMissilePositionX SPC %tempMissilePositionY;    
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
            AlienShipToy.IsExplode=false;
        }
        
    }
    
    else if((%distanceMissile2X<14 && %distanceMissile2X>0)&&(%distanceMissile2Y<10 && %distanceMissile2Y>0)&& %explode){
        echo("+++++++++++Enemy2");
        if(%enemy2PositionX!=null){
            %tempMissilePositionX=AlienShipToy.Missile.Position.x;
            %tempMissilePositionY=AlienShipToy.Missile.Position.y;

            if(%enemy2Type==0||%enemy2Type==1){
                AlienShipToy.ScoreValue+=10;
                %score=AlienShipToy.ScoreValue;
                AlienShipToy.Score.Text="Score " @ %score;
            }
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%tempMissilePositionX SPC %tempMissilePositionY;    
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
            AlienShipToy.IsExplode=false;
        }
        
    }
    else if((%distanceMissile3X<14 && %distanceMissile3X>0)&&(%distanceMissile3Y<10 && %distanceMissile3Y>0)&& %explode){
        echo("+++++++++++Enemy3");
        if(%enemy3PositionY!=null){
            %tempMissilePositionX=AlienShipToy.Missile.Position.x;
            %tempMissilePositionY=AlienShipToy.Missile.Position.y;

            if(%enemy3Type==0||%enemy3Type==1){
                AlienShipToy.ScoreValue+=10;
                %score=AlienShipToy.ScoreValue;
                AlienShipToy.Score.Text="Score " @ %score;

            }
            %particlePlayer = new ParticlePlayer();
            %particlePlayer.BodyType = "static";
            %particlePlayer.Position=%tempMissilePositionX SPC %tempMissilePositionY;    
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
            AlienShipToy.IsExplode=false;
        }
        
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
    
    %tempmissilecount=AlienShipToy.MissileCount;
    if(%possibleTime>2 && %tempmissilecount>0){
        //echo(%possibleTime);
        echo(%tempmissilecount);
        AlienShipToy.IsExplode=true;
        %missilePositionX= AlienShipToy.Player.Position.x;
        %missilePositionY=AlienShipToy.Player.Position.y;
        %missile=new Sprite();
        AlienShipToy.Missile=%missile;
        //%randomPosition = getRandom(-10, 10) SPC getRandom(2, 8);
        %missile.setImage("AlienShipToy:missile");
        %missile.setPosition(%missilePositionX,%missilePositionY-15);
        %missile.setSize(20, 7);
        %missile.setDefaultFriction(1.0);
        %missile.setDefaultDensity(0.1);
        %missile.createPolygonBoxCollisionShape(20, 10);
        %missile.setLinearVelocity(110,0);
        AlienShipToy.Player.SetAngularVelocity(0);
        SandboxScene.add( %missile );
        %music = alxPlay("AlienShipToy:missileSound");
        AlienShipToy.MissileHit=getSimTime();
        AlienShipToy.MissileCount--;
        %missilecount=AlienShipToy.MissileCount;
        AlienShipToy.MissileCountText.Text=%missilecount;
        
    }else{
        %missilePositionX= -200;
        %missilePositionY=-200;
        AlienShipToy.Missile.Position.x=%worldPosition.x;
        AlienShipToy.Missile.Position.y=%worldPosition.y-15;
    }
   
}

function AlienShipToy::onRightMouseDown(%this, %touchID, %worldPosition)
{
    AlienShipToy.create();
}



