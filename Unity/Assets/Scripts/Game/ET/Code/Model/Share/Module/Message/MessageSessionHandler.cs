using System;
using Cysharp.Threading.Tasks;

namespace ET
{
    public abstract class MessageSessionHandler<Message>: IMessageSessionHandler where Message : MessageObject
    {
        protected abstract UniTask Run(Session session, Message message);

        public void Handle(Session session, object msg)
        {
            HandleAsync(session, msg).Forget();
        }

        private async UniTask HandleAsync(Session session, object message)
        {
            using Message msg = message as Message;
            if (message == null)
            {
                session.Fiber().Error($"消息类型转换错误: {msg.GetType().FullName} to {typeof (Message).Name}");
                return;
            }

            if (session.IsDisposed)
            {
                session.Fiber().Error($"session disconnect {msg}");
                return;
            }

            await this.Run(session, msg);
        }

        public Type GetMessageType()
        {
            return typeof (Message);
        }

        public Type GetResponseType()
        {
            return null;
        }
    }
    
    
    public abstract class MessageSessionHandler<Request, Response>: IMessageSessionHandler where Request : MessageObject, IRequest where Response : MessageObject, IResponse
    {
        protected abstract UniTask Run(Session session, Request request, Response response);

        public void Handle(Session session, object message)
        {
            HandleAsync(session, message).Forget();
        }

        private async UniTask HandleAsync(Session session, object message)
        {
            try
            {
                using Request request = message as Request;
                if (request == null)
                {
                    throw new Exception($"消息类型转换错误: {message.GetType().FullName} to {typeof (Request).FullName}");
                }

                int rpcId = request.RpcId;
                long instanceId = session.InstanceId;

                Response response = ObjectPool.Instance.Fetch<Response>();
                Fiber fiber = session.Fiber();
                try
                {
                    await this.Run(session, request, response);
                }
                catch (RpcException exception)
                {
                    // 这里不能返回堆栈给客户端
                    fiber.Error(exception.ToString());
                    response.Error = exception.Error;
                }
                catch (Exception exception)
                {
                    // 这里不能返回堆栈给客户端
                    fiber.Error(exception.ToString());
                    response.Error = ErrorCore.ERR_RpcFail;
                }
                
                // 等回调回来,session可以已经断开了,所以需要判断session InstanceId是否一样
                if (session.InstanceId != instanceId)
                {
                    return;
                }
                
                response.RpcId = rpcId; // 在这里设置rpcId是为了防止在Run中不小心修改rpcId字段
                session.Send(response);
            }
            catch (Exception e)
            {
                throw new Exception($"解释消息失败: {message.GetType().FullName}", e);
            }
        }

        public Type GetMessageType()
        {
            return typeof (Request);
        }

        public Type GetResponseType()
        {
            return typeof (Response);
        }
    }
}