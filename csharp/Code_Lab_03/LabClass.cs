using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Code_Lab_03 {

	public class LabClass {

		/// <summary>
		/// 该方法会在当前的图纸空间的(0,0,0)点位上绘制一个十字标靶。
		/// </summary>
		[CommandMethod("CreateAimShape")]
		public void CreateAimShape() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = doc.Database;
			Editor editor = doc.Editor;
			#endregion

			//在控制台上输出绘制图形提示。
			editor.WriteMessage("正在向(0,0,0)点绘制图形...");
			//调用绘制图形子方法，绘制图形。
			DrawAimShape(database, new Point3d(0, 0, 0));
		}

		/// <summary>
		/// 该方法会询问用户一个点的坐标，并以用户指定的点为中心绘制一个十字标靶。
		/// </summary>
		[CommandMethod("PickAimShape")]
		public void PickAimShape() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = doc.Database;
			Editor editor = doc.Editor;
			#endregion

			//新建提示选项对象。
			PromptPointOptions options = new PromptPointOptions("请选择中心点...");
			//控制台通过指定的提示对象返回一个点提示结果。
			PromptPointResult result = editor.GetPoint(options);

			//判断结果的状态是否是OK，如果不是，则退出。
			if (result.Status != PromptStatus.OK) {
				editor.WriteMessage("无效的选择... 有可能是取消或是其它原因，方法中止...");
				return;
			}

			//调用绘制图形子方法，绘制图形。
			DrawAimShape(database, result.Value);
		}

		/// <summary>
		/// 向指定的数据库中以指定的点位为中心绘制十字标靶。
		/// </summary>
		/// <param name="database">指定的数据库。</param>
		/// <param name="centerPoint">指定的中心点。</param>
		private void DrawAimShape(Database database, Point3d centerPoint) {
			#region 入口参数非空判定。
			//入口参数非空判定。
			if (database == null) {
				throw new NullReferenceException("入口参数 database 不能为 null。");
			}
			if (centerPoint == null) {
				throw new NullReferenceException("入口参数 centerPoint 不能为 null。");
			}
			#endregion

			//开启事务。
			Transaction transaction = database.TransactionManager.StartTransaction();
			try {
				//使用当前的空间ID来获取块记录表——注意打开方式是写入方式。
				BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(database.CurrentSpaceId, OpenMode.ForWrite);

				//新建圆对象。
				Circle circle = new Circle(centerPoint, Vector3d.ZAxis, 2);
				//向区块记录表中写入新建的圆对象。
				btr.AppendEntity(circle);
				//通知事务在之前的操作中已经向数据库中添加了圆。
				transaction.AddNewlyCreatedDBObject(circle, true);

				//新建直线对象。
				Line line = new Line(new Point3d(centerPoint.X + 3, centerPoint.Y, centerPoint.Z), new Point3d(centerPoint.X - 3, centerPoint.Y, centerPoint.Z));
				//向区块记录表中写入新建的直线对象。
				btr.AppendEntity(line);
				//通知事务在之前的操作中已经向数据库中添加了直线。
				transaction.AddNewlyCreatedDBObject(line, true);

				//更新直线对象。
				line = new Line(new Point3d(centerPoint.X, centerPoint.Y + 3, centerPoint.Z), new Point3d(centerPoint.X, centerPoint.Y - 3, centerPoint.Z));
				//向区块记录表中写入更新的直线对象。
				btr.AppendEntity(line);
				//通知事务在之前的操作中已经向数据库中添加了直线。
				transaction.AddNewlyCreatedDBObject(line, true);

				//提交事务。
				transaction.Commit();
			} catch {
				//如果操作过程中产生了任何异常，则中止事务。
				transaction.Abort();
			} finally {
				//在final语句块中释放事务。
				transaction.Dispose();
			}
		}

	}
}
