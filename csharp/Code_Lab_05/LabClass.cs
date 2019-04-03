using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace Code_Lab_05 {

	public class LabClass {

		/// <summary>
		/// 输出从数据库中找到的第一个元素的信息。
		/// </summary>
		[CommandMethod("FirstEntityInfo")]
		public void FirstEntityInfo() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = doc.Database;
			Editor editor = doc.Editor;
			#endregion

			//开启事务。
			Transaction transaction = database.TransactionManager.StartTransaction();
			try {
				//使用当前的空间ID来获取块记录表——注意打开方式是只读方式。
				BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(database.CurrentSpaceId, OpenMode.ForRead);

				//定义Entity实体，并进行非空判断。
				Entity entity = GetFirstEntity(transaction, btr);
				if (entity == null) {
					editor.WriteMessage("没有在当前区块记录表中找到任何实体!");
					return;
				}

				//输出实体的详细信息。
				ShowDBObjectMessage(entity, editor);

			} catch (Autodesk.AutoCAD.Runtime.Exception e) {
				editor.WriteMessage(e.Message);
				//如果操作过程中产生了任何异常，则中止事务。
				transaction.Abort();
			} finally {
				//在final语句块中释放事务。
				transaction.Dispose();
			}
		}

		/// <summary>
		/// 判断区块记录表是否拥有至少一个元素Entity元素，如果没有，则返回 null。
		/// </summary>
		/// <param name="transaction">执行当前操作的指定的事务</param>
		/// <param name="btr">指定的区块记录表。</param>
		/// <returns>指定的区块记录表的第一个Entity元素，如果没有，则返回 null。</returns>
		private Entity GetFirstEntity(Transaction transaction, BlockTableRecord btr) {
			Entity entity = null;
			//遍历btr，返回遇到的第一个Entity对象。
			foreach (ObjectId id in btr) {
				entity = transaction.GetObject(id, OpenMode.ForRead) as Entity;
				if (entity != null) {
					return entity;
				}
			}
			//如果遍历结束仍然没有符合条件的对象，则返回 null。
			return null;
		}

		/// <summary>
		/// 输出用户选定的元素的信息。
		/// </summary>
		[CommandMethod("ShowEntityInfo")]
		public void ShowEntityInfo() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = doc.Database;
			Editor editor = doc.Editor;
			#endregion

			//调用editor方法，获取用户指定的Entity。
			PromptEntityOptions options = new PromptEntityOptions("请指定需要显示信息的实体...\r\n");
			PromptEntityResult result = editor.GetEntity(options);
			//判断结果的类型是否是OK，不是OK则中断方法。
			if (result.Status != PromptStatus.OK) {
				editor.WriteMessage("用户中止或者其它异常，退出!\r\n");
			}

			//开启事务，为输出做准备。
			Transaction transaction = database.TransactionManager.StartTransaction();
			try {
				//获取用户选定的DBObject，并判断次DBObject是否为Entity。
				Entity entity = transaction.GetObject(result.ObjectId, OpenMode.ForRead) as Entity;
				if (entity == null) {
					editor.WriteMessage("选定的对象不是Entity实体，退出!");
					return;
				}

				//输出实体的详细信息。
				ShowDBObjectMessage(entity, editor);
			} catch (Autodesk.AutoCAD.Runtime.Exception e) {
				editor.WriteMessage(e.Message);
				//如果操作过程中产生了任何异常，则中止事务。
				transaction.Abort();
			} finally {
				//在final语句块中释放事务。
				transaction.Dispose();
			}
		}

		private void ShowDBObjectMessage(Entity entity, Editor editor) {
			#region 入口参数非空判断。
			if (entity == null) {
				throw new ArgumentNullException("入口参数 dbObject 不能为 null。");
			}
			if (editor == null) {
				throw new ArgumentNullException("入口参数 editor 不能为 null。");
			}
			#endregion

			//输出对象的开发者属性
			editor.WriteMessage("---------------开发者---------------\r\n");
			editor.WriteMessage(String.Format("对象的类型是: {0}\r\n", entity.GetType().ToString()));

			//输出对象的ID与Handler
			editor.WriteMessage("---------------Misc属性---------------\r\n");
			editor.WriteMessage(String.Format("对象的 id 是: {0}\r\n", entity.Id));
			editor.WriteMessage(String.Format("对象的 handler 是: {0}\r\n", entity.Handle));
			editor.WriteMessage("\r\n");

			//输出对象的部分常规信息
			editor.WriteMessage("---------------General属性---------------\r\n");
			editor.WriteMessage(String.Format("对象所在的图层的名称是: {0}\r\n", entity.Layer));
			editor.WriteMessage(String.Format("对象的线型名称是: {0}\r\n", entity.Linetype));
			#region 输出对象的颜色信息。
			Color color = entity.Color;
			switch (color.ColorMethod) {
				case ColorMethod.ByAci:
					editor.WriteMessage("对象的颜色类型是索引色\r\n");
					editor.WriteMessage(color.ColorName);
					break;
				case ColorMethod.ByBlock:
					editor.WriteMessage(String.Format("对象的颜色类型是ByBloc\r\n"));
					break;
				case ColorMethod.ByColor:
					if (color.HasBookName) {
						editor.WriteMessage(String.Format("对象的颜色类型是真彩色，其bookname是: {0}\r\n", color.BookName));
					} else {
						editor.WriteMessage("对象的颜色是真彩色\r\n");
					}
					break;
				case ColorMethod.ByLayer:
					editor.WriteMessage(String.Format("对象的颜色类型是ByLayer\r\n"));
					break;
				case ColorMethod.ByPen:
					editor.WriteMessage(String.Format("对象的颜色类型是ByPen，其bookname是: {0}\r\n", color.BookName));
					break;
				default:
					break;
			}
			#endregion


		}
	}
}
