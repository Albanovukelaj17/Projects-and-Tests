package gioco;

import java.awt.event.KeyEvent;
import java.util.ArrayList;
import java.util.List;

import name.panitz.game2d.FallingImage;
import name.panitz.game2d.Game;
import name.panitz.game2d.GameObj;
import name.panitz.game2d.ImageObject;
import name.panitz.game2d.TextObject;
import name.panitz.game2d.Vertex;

import static java.awt.event.KeyEvent.*;

public record gioco(FallingImage player, int width, int height, List<List<? extends GameObj>> goss,
                            List<ImageObject> walls,
                            List<TextObject> texts,
                    List<FallingImage>enemy,
                    int[]lifekills,
                    List<ImageObject>flame)
        implements Game {



    static ImageObject newWall(Vertex corner) {
        return new ImageObject(corner, new Vertex(0, 0),30.0,30.0,"rst.png",null);
    }
static FallingImage newEnemy(Vertex corner){
        return new FallingImage("minions.png",corner,new Vertex(-1,0));
}
static ImageObject newFlame(Vertex Corner,Vertex Velocity){

        return new ImageObject(Corner,Velocity,20.0,20.0,"flamme.png",null);
}
    static final int GRID_WIDTH = 34;

    static String level1 = """
		              e   e  ee
		          wwww
		       ee            e  eeeee
		   www
		                    www
		        eeee
		    ww             www
		                          wwww
		            
		                eee
		                  wwwwww
		         www\040\040\040\040\040
		ee            ee    e     ee
		   wwww     www\040\040\040\040\040\040\040\040\040\040\040
		                   
		        eeee         eee eeeee
		wwwwwwwwwwwwwwwwwwwwwwwwwwwwww\040
		wwwwwwwwwwwwwwwwwwwwwwwwwwwwww
		wwwwwwwwwwwwwwwwwwwwwwwwwwwwww""";



    public gioco() {
        this(new FallingImage("ultimoo.png",new Vertex(0, 0),new Vertex(0,0) ),
                900, 650,new ArrayList<>(),new ArrayList<>(),new ArrayList<>(),
                new ArrayList<>(),new int[]{10000,0},new ArrayList<>());
        init();

    }

//public static boolean standing=true;

    @Override
    public void doChecks() {

        if (player().pos().y > height()) {
            player().pos().moveTo(new Vertex(2 * GRID_WIDTH, height() - 80));
        }
         enemymove();
        touchRs();
kills();


    }
private void enemymove() {
    for (var x : enemy) {
        for (var y : walls) {
            if (x.pos().x <= 10) x.velocity().x = 2;
            if (x.pos().x >= 890) x.velocity().x = -2;
            //if (x.touches(y) && x.pos().x > 11 && x.pos().x< 889) ;x.velocity().x= x.velocity().x * (-1);
            if (x.touches(y)) {
                if (lifekills[1] <= 20) {
                    x.pos().moveTo(new Vertex(this.player.pos().x + 100, this.player.pos().y - 90));
                } else {
                    x.pos().moveTo(new Vertex(-15000, -1000));


                }


            }
        }
    }
    }




    private void touchRs() {

        boolean isStandingOnTop = false;
        for (var x : walls) {
            if (this.player.touches(x)) {
                this.player.stop();
            }
            if(this.player.isStandingOnTopOf(x))
                isStandingOnTop= true;
        }
        if(!isStandingOnTop && !this.player.isJumping)
 this.player.startJump(0.2);
}



    public boolean lost() {
        if (life()<=0) {

            lose();
        return true;}
        return false;
    }
    public void lose(){
        texts.clear();
        texts.add(new TextObject(new Vertex(450, 300), "Game Over"));
    }
public boolean won() {


    // for (var y : enemy) {
    //   if (y.pos().x < 0 && y.pos().x > 900 && y.pos().y < 0 && y.pos().y > 0) {
    if (lifekills[1] >= 100) {
        win();
        return true;
    }

    return false;
}

public void win(){
    texts.add(new TextObject(new Vertex(450, 300), "You Won"));
}



    @Override
    public void init() {

        walls.clear();
         sparafuoco();
        readLevel();
        initTexts();
        lost();
        powerUp();
        won();
        goss().add(enemy);
        goss().add(walls);
goss.add(flame);
        goss().add(texts);



}

    private void readLevel() {
        int l = 0;
        var lines = level1.split("\\n");
        for (String line : lines) {
            int col = 0;
            for (char c : line.toCharArray()) {
                switch (c) {
                    case 'w'->walls.add(newWall(new Vertex(col * GRID_WIDTH, l * GRID_WIDTH)));

                    case 'p'->player().pos().moveTo(new Vertex(col * GRID_WIDTH, l * GRID_WIDTH - 2));
                    case 'e'->enemy.add(newEnemy(new Vertex(col * GRID_WIDTH, l * GRID_WIDTH)));
                    case 'f'-> flame.add( newFlame(new Vertex(this.player.pos().x, this.player.pos().y), new Vertex(+0.5, 0)));
                }
                col++;
            }
            l++;
        }
    }
             public int life(){
        for(var x : enemy){
            if(x.touches(player)){
                lifekills[0]--;

                initTexts();
            }
        }

               return lifekills[0]--;
       }




    private void initTexts() {
        texts.clear();
        texts.add(new TextObject(new Vertex(50, 20), "Life: " + lifekills[0]));
        texts.add(new TextObject(new Vertex(50, 60), "Kills: " + lifekills[1]));





    }
public void kills(){
        for(var x : enemy){
            for(var y : flame){
                if(y.touches(x)){
                    lifekills[1]+=1;
                    initTexts();
                    x.pos().moveTo(new Vertex(880,this.player.pos().y));
                    y.pos().moveTo(new Vertex(1500,1000));

                }
            }
        }
}
public void powerUp(){
        if(lifekills[1]>50){
            player.startJump(-9.0);

        }
}
public void sparafuoco() {
    if (this.player.velocity().x >= 0) {
        flame.add(newFlame(new Vertex(this.player.pos().x, this.player.pos().y + 70), new Vertex(+0.5, 0)));
    } else {
        flame.add(newFlame(new Vertex(this.player.pos().x, this.player.pos().y + 70), new Vertex(-0.5, 0)));


    }
    for (var f : flame) {
        for (var w : walls) {

            if (f.touches(w)&& f.pos().y<500) {
                f.pos().moveTo(new Vertex(15000, 1000));

            }
        }
    }
}
    @Override
    public void keyPressedReaction(KeyEvent keyEvent) {
        switch (keyEvent.getKeyCode()) {
            case VK_RIGHT ->
                this.player.right();


            case VK_LEFT ->
                this.player.left();


           case VK_DOWN -> this.player.stop();
            case VK_UP -> this.player.jump();


            case VK_K -> this.sparafuoco();


        }
    }

            public static void main (String...args){
                new gioco().play();

            }
        }
