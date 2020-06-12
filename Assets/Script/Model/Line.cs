using UnityEngine;
using System.Collections;

public class Line {

	public Point a;
	public Point b;
	/**1:水平直连 0:垂直直连*/
	public int direct;

	public Line(Point aa,Point bb,int dir)
	{
		a = aa;
		b = bb;
		direct = dir;
	}

}
